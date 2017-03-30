using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void FileReceiveEventHandler(
        object sender,
        FileReceiveEventArgs e);

    public class FileReceiveEventArgs : EventArgs
    {
        private ReceiveFileManager _receiveFileManager;
        private object _tag;

        public FileReceiveEventArgs() { }

        public FileReceiveEventArgs(
            ReceiveFileManager receiveFileManager)
            : base()
        {
            _receiveFileManager = receiveFileManager;
        }

        public ReceiveFileManager ReceiveFileManager
        {
            get { return _receiveFileManager; }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
