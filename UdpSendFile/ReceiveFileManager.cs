using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;

namespace CSharpWin
{


    public class ReceiveFileManager : IDisposable
    {
        #region Fields

        private string _md5;
        private string _path;
        private string _tempFileName;
        private string _fileName;
        private string _fullName;
        private long _partCount;
        private int _partSize;
        private long _length;
        private IPEndPoint _remoteIP;
        private FileStream _fileStream;
        private object _tag;
        private bool _success;
        private Stream _safeStream;
        private DateTime _lastReceiveTime;
        private Timer _timer;
        private int _interval = 5000;
        private Dictionary<int, bool> _receiveFilePartList;
        private long _receivePartCount;

        private static object SyncLock = new object();
        private static readonly int ReceiveTimeout = 5000;
        private static readonly string FileTemptName = ".tmp";

        #endregion

        #region Constructors

        public ReceiveFileManager() { }

        public ReceiveFileManager(
            string md5,
            string path,
            string fileName,
            long partCount, 
            int partSize,
            long length,
            IPEndPoint remoteIP)
        {
            _md5 = md5;
            _path = path;
            _fileName = fileName;
            _partCount = partCount;
            _partSize = partSize;
            _length = length;
            _remoteIP = remoteIP;
            Create();
        }

        #endregion

        #region Events

        public event FileReceiveCompleteEventHandler ReceiveFileComplete;

        public event EventHandler ReceiveFileTimeout;

        #endregion

        #region Properties

        public string MD5
        {
            get { return _md5; }
        }

        public long PartCount
        {
            get { return _partCount; }
        }

        public string Name
        {
            get { return _fileName; }
        }

        public string FileName
        {
            get 
            {
                if (string.IsNullOrEmpty(_fullName))
                {
                    GetFileName();
                }
                return _fullName;
            }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public bool Success
        {
            get { return _success; }
        }

        public IPEndPoint RemoteIP
        {
            get { return _remoteIP; }
        }

        public bool Completed
        {
            get { return _partCount == _receivePartCount; }
        }

        private Stream FileStream
        {
            get { return _safeStream; }
        }

        private Dictionary<int, bool> ReceiveFilePartList
        {
            get { return _receiveFilePartList; }
        }

        private Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new Timer(
                        new TimerCallback(delegate(object obj)
                        {
                            TimeSpan ts = DateTime.Now - _lastReceiveTime;
                            if (ts.TotalMilliseconds > ReceiveTimeout)
                            {
                                _lastReceiveTime = DateTime.Now;
                                OnReceiveFileTimeout(EventArgs.Empty);
                            }
                        }),
                        null,
                        Timeout.Infinite,
                        _interval);
                }
                return _timer;
            }
        }

        #endregion

        #region Methods

        private void Create()
        {
            _tempFileName = string.Format(
                "{0}\\{1}{2}", _path, _fileName, FileTemptName);
            _fileStream = new FileStream(
                _tempFileName,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None,
                _partSize * 10,
                true);
            _safeStream = Stream.Synchronized(_fileStream);
            _receiveFilePartList = new Dictionary<int, bool>();
            for (int i = 0; i < _partCount; i++)
            {
                _receiveFilePartList.Add(i, false);
            }
        }

        public void Start()
        {
            _lastReceiveTime = DateTime.Now;
            Timer.Change(0, _interval);
        }

        public int GetNextReceiveIndex()
        {
            foreach (int index in ReceiveFilePartList.Keys)
            {
                if (ReceiveFilePartList[index] == false)
                {
                    return index;
                }
            }
            return -1;
        }

        public int ReceiveBuffer(int index, byte[] buffer)
        {
            _lastReceiveTime = DateTime.Now;
            if (ReceiveFilePartList[index])
            {
                return 0;
            }
            else
            {
                lock (SyncLock)
                {
                    ReceiveFilePartList[index] = true;
                    _receivePartCount++;
                }
                FileStream.Position = index * _partSize;
                FileStream.BeginWrite(
                    buffer,
                    0,
                    buffer.Length,
                    new AsyncCallback(EndWrite),
                    _receivePartCount);
                return buffer.Length;
            }
        }

        protected virtual void OnReceiveFileComplete(FileReceiveCompleteEventArgs e)
        {
            if (ReceiveFileComplete != null)
            {
                ReceiveFileComplete(this, e);
            }
        }

        protected virtual void OnReceiveFileTimeout(EventArgs e)
        {
            if (ReceiveFileTimeout != null)
            {
                ReceiveFileTimeout(this, e);
            }
        }

        private void EndWrite(IAsyncResult result)
        {
            if (FileStream == null)
            {
                return;
            }

            FileStream.EndWrite(result);

            long index = (long)result.AsyncState;
            if (index == _partCount)
            {
                Dispose();
                File.Move(_tempFileName, FileName);
                _success = _md5 == MD5Helper.CretaeMD5(FileName);
                OnReceiveFileComplete(
                    new FileReceiveCompleteEventArgs(_success));
            }
        }

        private void GetFileName()
        {
            _fullName = string.Format("{0}\\{1}", _path, _fileName);
            int nameIndex = 1;
            int index = _fileName.LastIndexOf('.');
            while (File.Exists(_fullName))
            {
                 _fullName = string.Format(
                     "{0}\\{1}", 
                     _path, 
                     _fileName.Insert(index, nameIndex.ToString("_0")));
                 nameIndex++;
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            if (_fileStream != null)
            {
                _safeStream.Flush();
                _safeStream.Close();
                _safeStream.Dispose();
                _safeStream = null;
                _fileStream = null;
            }
            if (_receiveFilePartList != null)
            {
                _receiveFilePartList.Clear();
                _receiveFilePartList = null;
            }
        }

        #endregion
    }
}
