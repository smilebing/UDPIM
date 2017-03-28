using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Model
{
    public class MyMessage
    {
        public string from
        {
            get;
            set;
        }

        public string to
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public string content
        {
            get;
            set;
        }

        public ConcurrentDictionary<string, MyIPEndPoint> users
        {
            get;
            set;
        }

        public LoginModel loginModel
        {
            get;
            set;
        }
    }
}
