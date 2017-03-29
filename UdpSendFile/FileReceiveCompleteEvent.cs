using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void FileReceiveCompleteEventHandler(
        object sender,
        FileReceiveCompleteEventArgs e);

    public class FileReceiveCompleteEventArgs : EventArgs
    {
        private bool _success;

        public FileReceiveCompleteEventArgs() { }

        public FileReceiveCompleteEventArgs(bool success)
            : base()
        {
            _success = success;
        }

        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }
    }
}
