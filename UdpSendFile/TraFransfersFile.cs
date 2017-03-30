using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    [Serializable]
    public class TraFransfersFile
    {
        private string _md5;
        private int _index;
        private byte[] _buffer;

        public TraFransfersFile(string md5, int index, byte[] buffer)
        {
            _md5 = md5;
            _index = index;
            _buffer = buffer;
        }

        public string MD5
        {
            get { return _md5; }
            set { _md5 = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
    }
}
