using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void ReadFileBufferEventHandler(
        object sender,
        ReadFileBufferEventArgs e);

    public class ReadFileBufferEventArgs : EventArgs
    {
        private int _index;
        private byte[] _buffer;

        public ReadFileBufferEventArgs(int index, byte[] buffer)
            : base()
        {
            _index = index;
            _buffer = buffer;
        }

        public int Index
        {
            get { return _index; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }
    }
}
