using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CSharpWin
{
    /* 作者：Starts_2000
     * 日期：2009-08-02
     * 网站：http://www.csharpwin.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
     */

    public class UdpReceiveFile : IDisposable
    {
        #region Fields

        private UdpPeer _udpPeer;
        private int _port = 8900;
        private Dictionary<string, ReceiveFileManager> _receiveFileManagerList;
        private object _syncLock = new object();

        #endregion

        #region Constructors

        public UdpReceiveFile(int port)
        {
            _port = port;
        }

        #endregion

        #region Events

        public event RequestSendFileEventHandler RequestSendFile;

        public event FileReceiveBufferEventHandler FileReceiveBuffer;

        public event FileReceiveEventHandler FileReceiveComplete;

        public event FileReceiveEventHandler FileReceiveCancel;

        #endregion

        #region Properties

        public UdpPeer UdpPeer
        {
            get
            {
                if (_udpPeer == null)
                {
                    _udpPeer = new UdpPeer(_port);
                    _udpPeer.ReceiveData += new ReceiveDataEventHandler(UdpPeerReceiveData);
                }
                return _udpPeer;
            }
        }

        public Dictionary<string, ReceiveFileManager> ReceiveFileManagerList
        {
            get
            {
                if (_receiveFileManagerList == null)
                {
                    _receiveFileManagerList = new Dictionary<string, ReceiveFileManager>(10);
                }
                return _receiveFileManagerList;
            }
        }

        public int Port
        {
            get { return _port; }
        }

        #endregion

        #region Methods

        public void Start()
        {
            UdpPeer.Start();
        }

        public void AcceptReceive(RequestSendFileEventArgs e)
        {
            TraFransfersFileStart traFransfersFileStart = e.TraFransfersFileStart;
            IPEndPoint remoteIP = e.RemoteIP;
            ResponeTraFransfersFile responeTraFransfersFile;
            if (e.Cancel)
            {
                responeTraFransfersFile = new ResponeTraFransfersFile(
                    traFransfersFileStart.MD5, 0, -1);
                Send((int)Command.ResponeSendFile, responeTraFransfersFile, remoteIP);
            }
            else
            {
                ReceiveFileManager receiveFileManager;
                if (!ReceiveFileManagerList.TryGetValue(
                    traFransfersFileStart.MD5,
                    out receiveFileManager))
                {
                    receiveFileManager = new ReceiveFileManager(
                       traFransfersFileStart.MD5,
                       e.Path,
                       traFransfersFileStart.FileName,
                       traFransfersFileStart.PartCount,
                       traFransfersFileStart.PartSize,
                       traFransfersFileStart.Length,
                       remoteIP);
                    receiveFileManager.ReceiveFileComplete += 
                        new FileReceiveCompleteEventHandler(
                        ReceiveFileManagerReceiveFileComplete);
                    receiveFileManager.ReceiveFileTimeout +=
                        new EventHandler(ReceiveFileManagerReceiveFileTimeout);
                    ReceiveFileManagerList.Add(
                        traFransfersFileStart.MD5,
                        receiveFileManager);
                    receiveFileManager.Start();
                }
                responeTraFransfersFile = new ResponeTraFransfersFile(
                    traFransfersFileStart.MD5, 0, 0);
                Send((int)Command.ResponeSendFile, responeTraFransfersFile, remoteIP);
            }
        }

        public void CancelReceive(string md5,IPEndPoint remoteIP)
        {
            ReceiveFileManager receiveFileManager;
            if (ReceiveFileManagerList.TryGetValue(
                md5,
                out receiveFileManager))
            {
                Send(
                    (int)Command.RequestCancelReceiveFile,
                    md5,
                    remoteIP);
                lock (_syncLock)
                {
                    ReceiveFileManagerList.Remove(md5);
                    receiveFileManager.Dispose();
                    receiveFileManager = null;
                }
            }
        }

        private void ReceiveFileManagerReceiveFileComplete(
            object sender, FileReceiveCompleteEventArgs e)
        {
            ReceiveFileManager receiveFileManager =
                sender as ReceiveFileManager;
            OnFileReceiveComplete(new FileReceiveEventArgs(receiveFileManager));
            ReceiveFileManagerList.Remove(receiveFileManager.MD5);
        }

        private void ReceiveFileManagerReceiveFileTimeout(
            object sender, EventArgs e)
        {
            ReceiveFileManager receiveFileManager =
                sender as ReceiveFileManager;
            ResponeTraFransfersFile responeTraFransfersFile =
                new ResponeTraFransfersFile(
                receiveFileManager.MD5, 
                0, 
                receiveFileManager.GetNextReceiveIndex());
            Send(
                (int)Command.ResponeSendFilePack, 
                responeTraFransfersFile,
                receiveFileManager.RemoteIP);
        }

        protected virtual void OnRequestSendFile(RequestSendFileEventArgs e)
        {
            if (RequestSendFile != null)
            {
                RequestSendFile(this, e);
            }
        }

        protected virtual void OnFileReceiveBuffer(FileReceiveBufferEventArgs e)
        {
            if (FileReceiveBuffer != null)
            {
                FileReceiveBuffer(this, e);
            }
        }

        protected virtual void OnFileReceiveComplete(FileReceiveEventArgs e)
        {
            if (FileReceiveComplete != null)
            {
                FileReceiveComplete(this, e);
            }
        }

        protected virtual void OnFileReceiveCancel(FileReceiveEventArgs e)
        {
            if (FileReceiveCancel != null)
            {
                FileReceiveCancel(this, e);
            }
        }

        private void Send(int messageID, object data, IPEndPoint remoteIP)
        {
            SendCell cell = new SendCell(messageID, data);
            UdpPeer.Send(cell, remoteIP);
        }

        private void UdpPeerReceiveData(object sender, ReceiveDataEventArgs e)
        {
            SendCell cell = new SendCell();
            cell.FromBuffer(e.Buffer);
            switch (cell.MessageID)
            {
                case (int)Command.RequestSendFile:
                    OnStartRecieve((TraFransfersFileStart)cell.Data, e.RemoteIP);
                    break;
                case (int)Command.RequestSendFilePack:
                    OnRecieveBuffer((TraFransfersFile)cell.Data, e.RemoteIP);
                    break;
                case (int)Command.RequestCancelSendFile:
                    OnRequestCancelSendFile(cell.Data.ToString(), e.RemoteIP);
                    break;
            }
        }

        private void OnRecieveBuffer(
            TraFransfersFile traFransfersFile,
            IPEndPoint remoteEP)
        {
            ReceiveFileManager receiveFileManager;

            if (!ReceiveFileManagerList.TryGetValue(
                traFransfersFile.MD5,
                out receiveFileManager))
            {
                return;
            }
            if (receiveFileManager != null)
            {
                ResponeTraFransfersFile responeTraFransfersFile;
                int size = receiveFileManager.ReceiveBuffer(
                    traFransfersFile.Index,
                    traFransfersFile.Buffer);

                if (receiveFileManager.Completed)
                {
                    responeTraFransfersFile = new ResponeTraFransfersFile(
                        traFransfersFile.MD5,
                        size,
                        -2);
                    Send(
                        (int)Command.ResponeSendFilePack, 
                        responeTraFransfersFile, 
                        remoteEP);
                }
                else
                {
                    responeTraFransfersFile = new ResponeTraFransfersFile(
                        traFransfersFile.MD5,
                        size,
                        receiveFileManager.GetNextReceiveIndex());
                    Send(
                        (int)Command.ResponeSendFilePack, 
                        responeTraFransfersFile, 
                        remoteEP);
                }
                OnFileReceiveBuffer(
                    new FileReceiveBufferEventArgs(
                    receiveFileManager,traFransfersFile.Buffer.Length));
            }
        }

        private void OnStartRecieve(
            TraFransfersFileStart traFransfersFileStart,
            IPEndPoint remoteEP)
        {
            OnRequestSendFile(
                new RequestSendFileEventArgs(
                traFransfersFileStart,
                remoteEP));
        }

        private void OnRequestCancelSendFile(string md5,IPEndPoint remoteIP)
        {
            ReceiveFileManager receiveFileManager;
            if (ReceiveFileManagerList.TryGetValue(
                md5,
                out receiveFileManager))
            {
                OnFileReceiveCancel(
                    new FileReceiveEventArgs(receiveFileManager));
                lock (_syncLock)
                {
                    ReceiveFileManagerList.Remove(md5);
                    receiveFileManager.Dispose();
                    receiveFileManager = null;
                }
            }
            else
            {
                FileReceiveEventArgs fe = new FileReceiveEventArgs();
                fe.Tag = md5;
                OnFileReceiveCancel(fe);
            }
            Send(
                (int)Command.ResponeCancelSendFile,
                "OK",
                remoteIP);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if(_udpPeer != null)
            {
                _udpPeer.Dispose();
                _udpPeer = null;
            }
            if (_receiveFileManagerList != null &&
                _receiveFileManagerList.Count > 0)
            {
                foreach (ReceiveFileManager  receiveFileManager 
                    in _receiveFileManagerList.Values)
                {
                    receiveFileManager.Dispose();
                }
                _receiveFileManagerList.Clear();
            }
        }

        #endregion
    }
}
