using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void FileSendBufferEventHandler(
        object sender,
        FileSendBufferEventArgs e);

    public class FileSendBufferEventArgs : EventArgs
    {
        private SendFileManager _sendFileManager;
        private int _size;

        public FileSendBufferEventArgs(
            SendFileManager sendFileManager,
            int size)
            : base()
        {
            _sendFileManager = sendFileManager;
            _size = size;
        }

        public SendFileManager SendFileManager
        {
            get { return _sendFileManager; }
        }

        public int Size
        {
            get { return _size; }
        }
    }
}
