using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;

namespace CSharpWin
{


    public class UdpSendFile : IDisposable
    {
        #region Fields

        private UdpPeer _udpPeer;
        private string _remoteIP = "127.0.0.1";
        private int _remotePort = 8900;
        private int _port = 8899;
        private Dictionary<string, SendFileManager> _sendFileManagerList;
        private object _syncLock = new object();

        #endregion

        #region Constructors

        public UdpSendFile(
            string remoteIP,
            int remotePort,
            int port) 
        {
            _remoteIP = remoteIP;
            _remotePort = remotePort;
            _port = port;
        }

        #endregion

        #region Events

        public event FileSendEventHandler FileSendAccept;

        public event FileSendEventHandler FileSendRefuse;

        public event FileSendEventHandler FileSendCancel;

        public event FileSendBufferEventHandler FileSendBuffer;

        public event FileSendEventHandler FileSendComplete;

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

        public IPEndPoint RemoteEP
        {
            get { return new IPEndPoint(IPAddress.Parse(_remoteIP), _remotePort); }
        }

        public int Port
        {
            get { return _port; }
        }

        public Dictionary<string, SendFileManager> SendFileManagerList
        {
            get
            {
                if (_sendFileManagerList == null)
                {
                    _sendFileManagerList = new Dictionary<string, SendFileManager>(10);
                }
                return _sendFileManagerList;
            }
        }

        #endregion

        #region Methods

        public void Start()
        {
            UdpPeer.Start();
        }

        public bool CanSend(SendFileManager sendFileManager)
        {
            return !SendFileManagerList.ContainsKey(sendFileManager.MD5);
        }

        public void SendFile(string fileName)
        {
            SendFileManager sendFileManager = new SendFileManager(fileName);
            if (SendFileManagerList.ContainsKey(sendFileManager.MD5))
            {
                throw new Exception(string.Format(
                    "文件 {0} 正在发送，不能发送重复的文件。",
                    fileName));
            }
            else
            {
                SendFileManagerList.Add(sendFileManager.MD5, sendFileManager);
                sendFileManager.ReadFileBuffer += new ReadFileBufferEventHandler(
                    SendFileManageReadFileBuffer);
                TraFransfersFileStart ts = new TraFransfersFileStart(
                    sendFileManager.MD5,
                    sendFileManager.Name,
                    Icon.ExtractAssociatedIcon(fileName).ToBitmap(),
                    sendFileManager.Length,
                    sendFileManager.PartCount,
                    sendFileManager.PartSize);
                Send((int)Command.RequestSendFile, ts);
            }
        }

        public void SendFile(SendFileManager sendFileManager, Image image)
        {
            if (SendFileManagerList.ContainsKey(sendFileManager.MD5))
            {
                throw new Exception(string.Format(
                    "文件 {0} 正在发送，不能发送重复的文件。",
                    sendFileManager.FileName));
            }
            else
            {
                SendFileManagerList.Add(sendFileManager.MD5, sendFileManager);
                sendFileManager.ReadFileBuffer += new ReadFileBufferEventHandler(
                    SendFileManageReadFileBuffer);
                TraFransfersFileStart ts = new TraFransfersFileStart(
                    sendFileManager.MD5,
                    sendFileManager.Name,
                    image,
                    sendFileManager.Length,
                    sendFileManager.PartCount,
                    sendFileManager.PartSize);
                Send((int)Command.RequestSendFile, ts);
            }
        }

        public void CancelSend(string md5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                md5,
                out sendFileManager))
            {
                Send(
                    (int)Command.RequestCancelSendFile,
                    md5);
                lock (_syncLock)
                {
                    SendFileManagerList.Remove(md5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
            }
        }

        protected virtual void OnFileSendAccept(FileSendEventArgs e)
        {
            if (FileSendAccept != null)
            {
                FileSendAccept(this, e);
            }
        }

        protected virtual void OnFileSendRefuse(FileSendEventArgs e)
        {
            if (FileSendRefuse != null)
            {
                FileSendRefuse(this, e);
            }
        }

        protected virtual void OnFileSendCancel(FileSendEventArgs e)
        {
            if (FileSendCancel != null)
            {
                FileSendCancel(this, e);
            }
        }

        protected virtual void OnFileSendComplete(FileSendEventArgs e)
        {
            if (FileSendComplete != null)
            {
                FileSendComplete(this, e);
            }
        }

        protected virtual void OnFileSendBuffer(FileSendBufferEventArgs e)
        {
            if (FileSendBuffer != null)
            {
                FileSendBuffer(this, e);
            }
        }

        private void SendFileManageReadFileBuffer(
            object sender, ReadFileBufferEventArgs e)
        {
            SendFileManager sendFileManager = sender as SendFileManager;
            TraFransfersFile ts = new TraFransfersFile(
                sendFileManager.MD5, e.Index, e.Buffer);
            Send((int)Command.RequestSendFilePack, ts);
        }

        private void Send(int messageID, object data, IPEndPoint remoteIP)
        {
            SendCell cell = new SendCell(messageID, data);
            byte[] buffer = cell.ToBuffer();
            UdpPeer.Send(cell, remoteIP);
        }

        private void Send(int messageID, object data)
        {
            Send(messageID, data, RemoteEP);
        }

        private void UdpPeerReceiveData(object sender, ReceiveDataEventArgs e)
        {
            SendCell cell = new SendCell();
            cell.FromBuffer(e.Buffer);
            switch (cell.MessageID)
            {
                case (int)Command.ResponeSendFile:
                    OnResponeSendFile((ResponeTraFransfersFile)cell.Data);
                    break;
                case (int)Command.ResponeSendFilePack:
                    ResponeSendFilePack((ResponeTraFransfersFile)cell.Data);
                    break;
                case (int)Command.RequestCancelReceiveFile:
                    RequestCancelReceiveFile(cell.Data.ToString());
                    break;
            }
        }

        private void OnResponeSendFile(ResponeTraFransfersFile data)
        {
            SendFileManager sendFileManager;
            if (!SendFileManagerList.TryGetValue(
                data.MD5,
                out sendFileManager))
            {
                return;
            }
            if (data.Size > 0)
            {
                OnFileSendBuffer(new FileSendBufferEventArgs(
                    sendFileManager,
                    data.Size));
            }
            if (data.Index == 0)
            {
                if (sendFileManager != null)
                {
                    OnFileSendAccept(new FileSendEventArgs(sendFileManager));
                    sendFileManager.Read(data.Index);
                }
            }
            else
            {
                if (data.Index == -1)
                {
                    OnFileSendRefuse(new FileSendEventArgs(sendFileManager));
                }
                SendFileManagerList.Remove(data.MD5);
                sendFileManager.Dispose();
            }
        }

        private void ResponeSendFilePack(ResponeTraFransfersFile data)
        {
            SendFileManager sendFileManager;
            if (!SendFileManagerList.TryGetValue(
                data.MD5,
                out sendFileManager))
            {
                return;
            }
            if (data.Size > 0)
            {
                OnFileSendBuffer(new FileSendBufferEventArgs(
                    sendFileManager,
                    data.Size));
            }
            if (data.Index >= 0)
            {
                if (sendFileManager != null)
                {
                    sendFileManager.Read(data.Index);
                }
            }
            else
            {
                if (data.Index == -1)
                {
                    OnFileSendRefuse(new FileSendEventArgs(sendFileManager));
                }
                else if (data.Index == -2)
                {
                    OnFileSendComplete(new FileSendEventArgs(sendFileManager));
                }
                SendFileManagerList.Remove(data.MD5);
                sendFileManager.Dispose();
            }
        }

        private void RequestCancelReceiveFile(string md5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                md5,
                out sendFileManager))
            {
                OnFileSendCancel(
                    new FileSendEventArgs(sendFileManager));
                lock (_syncLock)
                {
                    SendFileManagerList.Remove(md5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
            }

            Send(
                (int)Command.ResponeCancelReceiveFile,
                "OK");
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (_udpPeer != null)
            {
                _udpPeer.Dispose();
                _udpPeer = null;
            }
            if (_sendFileManagerList != null &&
                _sendFileManagerList.Count > 0)
            {
                foreach (SendFileManager sendFileManager 
                    in _sendFileManagerList.Values)
                {
                    sendFileManager.Dispose();
                }
                _sendFileManagerList.Clear();
            }
        }

        #endregion
    }
}
