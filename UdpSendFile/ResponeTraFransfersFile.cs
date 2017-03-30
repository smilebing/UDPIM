using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    [Serializable]
    public class ResponeTraFransfersFile
    {
        private string _md5;
        private int _size;
        private int _index;

        public ResponeTraFransfersFile() { }

        public ResponeTraFransfersFile(string md5, int size, int index)
        {
            _md5 = md5;
            _size = size;
            _index = index;
        }

        public string MD5
        {
            get { return _md5; }
            set { _md5 = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
