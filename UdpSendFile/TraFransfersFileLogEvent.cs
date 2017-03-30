using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public delegate void TraFransfersFileLogEventHandler(
        object sender,
        TraFransfersFileLogEventArgs e);

    public class TraFransfersFileLogEventArgs : EventArgs
    {
        private string _messgae;

        public TraFransfersFileLogEventArgs(string message)
            : base()
        {
            _messgae = message;
        }

        public string Message
        {
            get { return _messgae; }
        }
    }
}
