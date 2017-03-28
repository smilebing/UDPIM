using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Model;
using System.Data.OleDb;
namespace UDPIM.Socket
{
    class Server
    {
        static Server instance=null;
        UdpClient udpClient =null;
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8800);

        Access access;
        OleDbConnection conn;
        static string exePath = @"C:\Users\zhuda\Documents\Visual Studio 2013\Projects\UDPIM";//本程序所在路径

        private Server()
        {
            //创建连接对象
            conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + exePath + @"\IMDB.mdb");
            access = new Access(conn);
            access.openConn();
           
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
                    Console.WriteLine(msg);

                    //处理接收过来的数据
                    handleMsg(msg, ip);
                    udpClient.BeginReceive(EndReceive, s);//在这里重新开始一个异步接收，用于处理下一个网络请求
                }
            }
            catch (Exception ex)
            {
                //处理异常
                Console.WriteLine("server error" + ex.Message);
            }
        }


        /// <summary>
        /// 处理接收过来的数据，msg 为json类型
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="remoteIPEndPoint"></param>
        private void handleMsg(string msgStr, IPEndPoint remoteIPEndPoint)
        {
            //{
            //    "from":"username",
            //    "to":"username",
            //    "type":"text/img/file",
            //    "content":"......"
            //}

            //获取消息类
            MyMessage receiveMsg = JsonConvert.DeserializeObject<MyMessage>(msgStr);

            switch(receiveMsg.type)
            {
                case "heart":
                    //心跳包
                    MyIPEndPoint myIPEndPoint = new MyIPEndPoint();
                    myIPEndPoint.ip = remoteIPEndPoint.Address.ToString();
                    myIPEndPoint.port = remoteIPEndPoint.Port;

                    OnlineUsers onlineUsers = OnlineUsers.getInstance();
                    onlineUsers.users.AddOrUpdate(receiveMsg.from,myIPEndPoint,(k,v)=>v);
                    //将在线用户的信息反馈给用户

                    //构造消息
                    MyMessage msgObject = new MyMessage();
                    msgObject.from = "server";
                    msgObject.to = "client";
                    msgObject.type = "heartFeedBack";
                    msgObject.content = "";
                    msgObject.users = onlineUsers.users;
                    sendMsg(JsonConvert.SerializeObject(msgObject), remoteIPEndPoint);
                    Console.WriteLine("server send:" + JsonConvert.SerializeObject(msgObject));

                    break;
                case "login":
                    
                    break;
                case "register":
                    break;
            }
           

        }

        public void sendMsg(String msgStr, IPEndPoint remoteIPEndPoint)
        {
            byte[] msg = Encoding.ASCII.GetBytes(msgStr);
            udpClient.SendAsync(msg, msg.Length, remoteIPEndPoint);
        }
    }
}
