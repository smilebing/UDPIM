using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    public enum Command
    {
        RequestSendFile = 0x000001,
        ResponeSendFile = 0x100001,

        RequestSendFilePack = 0x000002,
        ResponeSendFilePack = 0x100002,

        RequestCancelSendFile = 0x000003,
        ResponeCancelSendFile = 0x100003,

        RequestCancelReceiveFile = 0x000004,
        ResponeCancelReceiveFile = 0x100004,
    }
}
