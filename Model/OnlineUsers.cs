using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Net;

namespace Model
{
    public class OnlineUsers
    {
        private static OnlineUsers instance=null;

        public  ConcurrentDictionary <string , MyIPEndPoint> users;
        private OnlineUsers()
        {
            users=new ConcurrentDictionary<string,MyIPEndPoint>();
        }

        public static OnlineUsers getInstance()
        {
            if(instance==null)
            {
                instance = new OnlineUsers();
            }
            return instance;
        }

    }
}
