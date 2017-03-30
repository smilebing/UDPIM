using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace Model
{
    public class ServerIP
    {
        public static string serverIP = "192.168.16.103";
        public static int serverPort = 8800;

        public static IPEndPoint getServerIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        }

        public static MyIPEndPoint getServerMyIPEndPoint()
        {
            MyIPEndPoint serverIPEndPoint = new MyIPEndPoint();
            serverIPEndPoint.ip = serverIP;
            serverIPEndPoint.port = serverPort;

            return serverIPEndPoint;
        }
    }
}
