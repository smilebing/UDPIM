using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    internal class ReadFileObject
    {
        private int _index;
        private byte[] _buffer;

        public ReadFileObject(int index, byte[] buffer)
        {
            _index = index;
            _buffer = buffer;
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
