using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{
    public interface IDataCell
    {
        byte[] ToBuffer();

        void FromBuffer(byte[] buffer);
    }
}
