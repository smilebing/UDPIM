using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{
  

    public class LabelClickEventArgs : EventArgs
    {
        private int _flag;
        public int Flag
        {
            get { return _flag; }
        }

        public LabelClickEventArgs(int flag)
        {
            _flag = flag;
        }

        public LabelClickEventArgs() { }
    }

    public delegate void LabelClickEventHandler(
        object sender,
        LabelClickEventArgs e);
}
