using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Newtonsoft.Json;
using System.Windows.Forms;
using Model;
using System.Windows.Forms;


namespace UDPIMClient.Socket
{
    
    class Server
    {
        public delegate void myDelegate( Login loginForm );

        //设置服务器的地址和端口
        static string SERVER_IP = "127.0.0.1";
        static int SERVER_PORT = 8800;
        static int HEART_BEAT_SLEEP_TIME = 1000 * 5;

        //当前登录的用户名
        public string currentUsername
        {
            get;
            set;
        }

        static Server instance=null;
        UdpClient udpClient =null;
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 6600);


        private Server()
        {
            udpClient = new UdpClient(RemoteIpEndPoint);
            UdpState s = new UdpState(udpClient, RemoteIpEndPoint);
            udpClient.BeginReceive(EndReceive, s);
        }

        /// <summary>
        /// 单利
        /// </summary>
        /// <returns></returns>
        public static Server getInstance()
        {
            if(instance==null)
            {
                instance = new Server();
            }
            return instance;
        }

        public void start()
        {
            
        }

        public void  stop()
        {
            udpClient.Close();
        }

        /// <summary>
        /// 处理接收过来的数据，msg 为json类型
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="remoteIPEndPoint"></param>
        private void handleMsg(string msg,IPEndPoint remoteIPEndPoint)
        {
            //{
            //    "from":"username",
            //    "to":"username",
            //    "type":"text/img/file",
            //    "content":"......"
            //}
            
            //获取消息类
            MyMessage receiveMsg = JsonConvert.DeserializeObject < MyMessage >( msg);

            switch(receiveMsg.type)
            {
                case"heartFeedBack":
                    Console.WriteLine("client receive heartFeedBack");
                    Console.WriteLine(msg);
                    //发送心跳包给服务器后服务器的反馈
                    OnlineUsers.getInstance().users = receiveMsg.users;
                    break;

                case "login":
                    //登录类型的信息
                    if(receiveMsg.content=="true")
                    {
                        currentUsername = receiveMsg.to;
                        startHeartBeatThread();

                        myDelegate myDelegate = new myDelegate(delegate(Login loginForm)
                        {
                            UserList uForm = UserList.getInstance();
                            uForm.Show();
                            loginForm.Hide();
                        });

                        Login lForm = Login.getInstance();

                        lForm.Invoke(myDelegate,lForm);
                        //登录成功

                  

                      
                        
                    }
                    else
                    {
                        //登录失败
                        MessageBox.Show("登录失败");
                    }
                    break;
                case "register":
                    //注册类型的信息
                    break;

                case "chat":
                    //普通的聊天信息
                    //获取所有的打开窗体
                    FormCollection collections = Application.OpenForms;
                    foreach (Form eachForm in collections)
                    {
                        //找出聊天窗口
                        if (eachForm.GetType().Equals(typeof(ChatForm)))
                        {
                            ChatForm chatForm = (ChatForm)eachForm;
                            Console.WriteLine("找到聊天窗口");
                            //如果对话框已经打开
                            if (receiveMsg.from == chatForm.username)
                            {
                                chatForm.addTips(receiveMsg.content);
                            }
                            else
                            {
                                //对话窗没有打开
                                ChatForm newChatForm = new ChatForm();
                                //设置窗体内的具体信息
                                newChatForm.username = receiveMsg.from;
                                newChatForm.remoteIPEndPoint = remoteIPEndPoint;

                                newChatForm.Show();
                                newChatForm.addTips(receiveMsg.content);
                            }
                        }
                    }
                    break;
            }

           
            
        }
        private  void EndReceive(IAsyncResult ar)
        {
            try
            {
                UdpState s = ar.AsyncState as UdpState;
                if (s != null)
                {
                    UdpClient udpClient = s.udpClient;

                    IPEndPoint ip = s.ip;
                    Byte[] receiveBytes = udpClient.EndReceive(ar, ref ip);

                    string msg = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine("client 收到信息");

                    //处理接收过来的数据
                    handleMsg(msg, ip);
                    udpClient.BeginReceive(EndReceive, s);//在这里重新开始一个异步接收，用于处理下一个网络请求
                }
            }
            catch (Exception ex)
            {
                //处理异常
                Console.WriteLine("client error"+ex.Message);
            }
        }


        public void sendMsg(MyMessage msg, IPEndPoint remoteIPEndPoint)
        {
            string msgStr = JsonConvert.SerializeObject(msg);

            byte[] sendBytes = Encoding.ASCII.GetBytes(msgStr);
            Console.WriteLine("server 发送" + sendBytes.Length + "byte");
            udpClient.SendAsync(sendBytes, sendBytes.Length, remoteIPEndPoint);
        }

        public void startHeartBeatThread()
        {
            Thread heartBeatThread = new Thread(sendHeartBeatMsg);
            heartBeatThread.Start();
        }

        public void sendHeartBeatMsg()
        {
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);
            MyMessage heartBeatMsg = new MyMessage();
            heartBeatMsg.from = currentUsername;
            heartBeatMsg.to = "server";
            heartBeatMsg.type = "heart";
            heartBeatMsg.content = "heart";

            while(true)
            {
                sendMsg(heartBeatMsg, serverIPEndPoint);
                Thread.Sleep(HEART_BEAT_SLEEP_TIME);
            }
        }

        public void showUserListForm()
        {
            UserList userListForm = UserList.getInstance();
            userListForm.Show();

        }

        public void show()
        {
         
        }

        
    }
}
