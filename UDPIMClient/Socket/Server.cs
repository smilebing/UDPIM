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

namespace UDPIMClient.Socket
{

    class Server
    {
        public delegate void myDelegate(Login loginForm);
        public delegate void showChatFormDelegate(string username, IPEndPoint remoteIPEndPoint);
        public delegate void AddTipsDelegate(ChatForm chatForm,string msg);
        //设置服务器的地址和端口

        static int HEART_BEAT_SLEEP_TIME = 1000 * 5;

        //当前登录的用户名
        public string currentUsername
        {
            get;
            set;
        }

        static Server instance = null;
        UdpClient udpClient = null;
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
            if (instance == null)
            {
                instance = new Server();
            }
            return instance;
        }

        public void start()
        {

        }

        public void stop()
        {
            udpClient.Close();
        }

        /// <summary>
        /// 处理接收过来的数据，msg 为json类型
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="remoteIPEndPoint"></param>
        private void handleMsg(string msg, IPEndPoint remoteIPEndPoint)
        {
            //{
            //    "from":"username",
            //    "to":"username",
            //    "type":"text/img/file",
            //    "content":"......"
            //}

            //获取消息类
            MyMessage receiveMsg = JsonConvert.DeserializeObject<MyMessage>(msg);

            switch (receiveMsg.type)
            {
                #region heartFeedBack 服务器反馈心跳信息
                case "heartFeedBack":
                    Console.WriteLine("client receive heartFeedBack");
                    Console.WriteLine(msg);
                    //发送心跳包给服务器后服务器的反馈
                    OnlineUsers.getInstance().users = receiveMsg.users;
                    break;
                #endregion

                #region chat 聊天信息
                case "chat":
                    Console.WriteLine("client 收到" + receiveMsg.content);
                    //普通的聊天信息
                    //显示聊天窗口或者在已经打开的窗口中添加信息
                    showChatForm(receiveMsg.from,receiveMsg.content, remoteIPEndPoint); 
                    break;
                #endregion

                #region file 文件发送信息
                case "file":
                    if (receiveMsg.content == "prepared")
                    {
                        //请求发送文件后收到的反馈
                        //显示发送文件的窗口
                        showSendFileForm(receiveMsg.from, remoteIPEndPoint);
                    }
                    else if (receiveMsg.content == "send")
                    {
                        //对反请求发送文件
                        //查看发送者信息，显示接收文件的窗口
                        showChatForm(receiveMsg.from, "向您发送文件", remoteIPEndPoint, true);
                        //发送接收文件准备完毕的信息
                        MyMessage fileReadyMsg = new MyMessage();
                        fileReadyMsg.from = currentUsername;
                        fileReadyMsg.to = receiveMsg.from;
                        fileReadyMsg.type = "file";
                        fileReadyMsg.content = "prepared";

                        sendMsg(fileReadyMsg, remoteIPEndPoint);
                    }
                   
                    break;
                #endregion

                #region login 登录信息
                case "login":
                    //登录类型的信息
                    if (receiveMsg.content == "true")
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

                        lForm.Invoke(myDelegate, lForm);
                        //登录成功





                    }
                    else
                    {
                        //登录失败
                        MessageBox.Show("登录失败");
                    }
                    break;
                #endregion

                #region register 注册信息
                case "register":
                    //注册类型的信息
                    break;
                #endregion

          
            }



        }
        private void EndReceive(IAsyncResult ar)
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
                Console.WriteLine("client error" + ex.Message);
            }
        }


        public void sendMsg(MyMessage msg, IPEndPoint remoteIPEndPoint)
        {
            string msgStr = JsonConvert.SerializeObject(msg);

            byte[] sendBytes = Encoding.UTF8.GetBytes(msgStr);
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
            MyMessage heartBeatMsg = new MyMessage();
            heartBeatMsg.from = currentUsername;
            heartBeatMsg.to = "server";
            heartBeatMsg.type = "heart";
            heartBeatMsg.content = "heart";

            while (true)
            {
                sendMsg(heartBeatMsg, ServerIP.getServerIPEndPoint());
                Thread.Sleep(HEART_BEAT_SLEEP_TIME);
            }
        }

        public void showUserListForm()
        {
            UserList userListForm = UserList.getInstance();
            userListForm.Show();
        }

        /// <summary>
        /// 将信息在聊天窗口显示
        /// </summary>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <param name="remoteIPEndPoint"></param>
        public void showChatForm(string msgFrom,string msg,IPEndPoint remoteIPEndPoint,bool showReceiveFileForm=false)
        {
            //获取所有的打开窗体
            FormCollection collections = Application.OpenForms;
            foreach (Form eachForm in collections)
            {
                //找出聊天窗口
                if (eachForm.GetType().Equals(typeof(ChatForm)))
                {
                    ChatForm chatForm = (ChatForm)eachForm;
                    //如果对话框已经打开
                    if (msgFrom == chatForm.username)
                    {
                        AddTipsDelegate addTipsDelegate = new AddTipsDelegate(delegate(ChatForm form, string s)
                        {
                            form.addTips(s);

                            //判断是否要打开传输文件界面
                            if(showReceiveFileForm)
                            {
                                ReceiveFileForm receiveFileForm = new ReceiveFileForm();
                                receiveFileForm.Show();
                            }
                        });
                        Console.WriteLine("找到聊天窗口");

                        chatForm.Invoke(addTipsDelegate, chatForm, msg);
                        return;
                    }

                }
            }


            UserList userListForm = UserList.getInstance();
            showChatFormDelegate showDelegate = new showChatFormDelegate(delegate(string username, IPEndPoint remoteIP)
            {
                //对话窗没有打开
                ChatForm newChatForm = new ChatForm();
                //设置窗体内的具体信息
                newChatForm.Text = username;
                newChatForm.username = username;
                newChatForm.remoteIPEndPoint = remoteIP;
                newChatForm.Show();
                newChatForm.addTips(msg);

                //判断是否要打开传输文件界面
                if(showReceiveFileForm)
                {
                    ReceiveFileForm receiveFileForm = new ReceiveFileForm();
                    receiveFileForm.Show();
                }
            });

            userListForm.Invoke(showDelegate, msgFrom, remoteIPEndPoint);
        }

        /// <summary>
        /// 显示发送文件的窗口
        /// </summary>
        /// <param name="msgFrom"></param>
        /// <param name="remoteIPEndPoint"></param>
        public void showSendFileForm(string msgFrom,IPEndPoint remoteIPEndPoint)
        {
            //获取所有的打开窗体
            FormCollection collections = Application.OpenForms;
            foreach (Form eachForm in collections)
            {
                //找出聊天窗口
                if (eachForm.GetType().Equals(typeof(ChatForm)))
                {
                    ChatForm chatForm = (ChatForm)eachForm;
                    //如果对话框已经打开
                    if (msgFrom == chatForm.username)
                    {
                        AddTipsDelegate addTipsDelegate = new AddTipsDelegate(delegate(ChatForm form, string s)
                        {
                            form.addTips(s);
                            //显示发送文件的窗口
                            SendFileForm sendFileForm = SendFileForm.getInstance();
                            sendFileForm.remoteIPEndPoint = remoteIPEndPoint;
                            sendFileForm.remoteUsername = msgFrom;
                            sendFileForm.Show(); ;
                           
                        });

                        chatForm.Invoke(addTipsDelegate, chatForm, "你请求发送文件\n");
                        return;
                    }

                }
            }


            UserList userListForm = UserList.getInstance();
            showChatFormDelegate showDelegate = new showChatFormDelegate(delegate(string username, IPEndPoint remoteIP)
            {
                //对话窗没有打开
                ChatForm newChatForm = new ChatForm();
                //设置窗体内的具体信息
                newChatForm.Text = username;
                newChatForm.username = username;
                newChatForm.remoteIPEndPoint = remoteIP;
                newChatForm.Show();
                newChatForm.addTips("你请求发送文件");

                //打开文件传输form
                //显示发送文件的窗口
                SendFileForm sendFileForm = SendFileForm.getInstance();
                sendFileForm.remoteIPEndPoint = remoteIPEndPoint;
                sendFileForm.remoteUsername = msgFrom;
                sendFileForm.Show();
                
            });

            userListForm.Invoke(showDelegate, msgFrom, remoteIPEndPoint);
        }


    }
}
