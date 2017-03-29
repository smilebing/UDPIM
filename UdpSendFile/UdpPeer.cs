using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace CSharpWin
{

    public class UdpPeer : IDisposable
    {
        private UdpClient _udpClient;
        private int _port = 8899;
        private bool _started;

        public UdpPeer(int port)
        {
            _port = port;
        }

        public event ReceiveDataEventHandler ReceiveData;

        public int Port
        {
            get { return _port; }
        }

        internal UdpClient UdpClient
        {
            get
            {
                if (_udpClient == null)
                {
                    bool success = false;
                    while (!success)
                    {
                        try
                        {
                            _udpClient = new UdpClient(_port);
                            success = true;
                        }
                        catch(SocketException ex)
                        {
                            _port++;
                            if (_port > 65535)
                            {
                                success = true;
                                throw ex;
                            }
                        }
                    }

                    uint IOC_IN = 0x80000000;
                    uint IOC_VENDOR = 0x18000000;
                    uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                    _udpClient.Client.IOControl(
                        (int)SIO_UDP_CONNRESET,
                        new byte[] { Convert.ToByte(false) },
                        null);
                }
                return _udpClient;
            }
        }

        public void Start()
        {
            if (!_started)
            {
                _started = true;
                ReceiveInternal();
            }
        }

        public void Send(IDataCell cell, IPEndPoint remoteIP)
        {
            byte[] buffer = cell.ToBuffer();
            SendInternal(buffer, remoteIP);
        }

        protected void SendInternal(byte[] buffer,IPEndPoint remoteIP)
        {
            if (!_started)
            {
                throw new ApplicationException("UDP Closed.");
            }
            try
            {
                UdpClient.BeginSend(
                   buffer,
                   buffer.Length,
                   remoteIP,
                   new AsyncCallback(SendCallback),
                   null);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        protected void ReceiveInternal()
        {
            if (!_started)
            {
                return;
            }
            try
            {
                UdpClient.BeginReceive(
                   new AsyncCallback(ReceiveCallback),
                   null);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                UdpClient.EndSend(result);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            if (!_started)
            {
                return;
            }
            IPEndPoint remoteIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = null;
            try
            {
                buffer = UdpClient.EndReceive(result, ref remoteIP);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            finally
            {
                ReceiveInternal();
            }

            OnReceiveData(new ReceiveDataEventArgs(buffer, remoteIP));
        }

        protected virtual void OnReceiveData(ReceiveDataEventArgs e)
        {
            if(ReceiveData != null)
            {
                ReceiveData(this, e);
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _started = false;
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null;
            }
        }

        #endregion
    }
}
