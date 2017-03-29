using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void FileReceiveBufferEventHandler(
        object sender,
        FileReceiveBufferEventArgs e);

    public class FileReceiveBufferEventArgs : EventArgs
    {
        private ReceiveFileManager _receiveFileManager;
        private int _size;

        public FileReceiveBufferEventArgs(
            ReceiveFileManager receiveFileManager,
            int size)
            : base()
        {
            _receiveFileManager = receiveFileManager;
            _size = size;
        }

        public ReceiveFileManager ReceiveFileManager
        {
            get { return _receiveFileManager; }
        }

        public int Size
        {
            get { return _size; }
        }
    }
}
