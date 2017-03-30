using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Model
{
    public class ControlTag
    {
        private string _md5;
        private string _fileName;
        private IPEndPoint _remoteIP;

        public ControlTag(
            string md5,
            string fileName,
            IPEndPoint remoteIP)
        {
            _md5 = md5;
            _fileName = fileName;
            _remoteIP = remoteIP;
        }

        public string MD5
        {
            get { return _md5; }
            set { _md5 = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public IPEndPoint RemoteIP
        {
            get { return _remoteIP; }
            set { _remoteIP = value; }
        }
    }
}
