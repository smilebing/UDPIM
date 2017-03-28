using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Model
{
    public class UdpState
    {

        public UdpClient udpClient
        {
            get;
            set;
        }


        public IPEndPoint ip
        {
            get;
            set;
        }

        public UdpState(UdpClient udpclient, IPEndPoint ip)
        {
            this.udpClient = udpclient;
            this.ip = ip;
        }
    }
}
