using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace CSharpWin
{


    [Serializable]
    public class TraFransfersFileStart
    {
        private string _md5; 
        private string _fileName;
        private Image _image;
        private long _length;
        private long _partCount;
        private int _partSize;

        public TraFransfersFileStart(
            string md5,
            string fileName,
            Image image,
            long length,
            long partCount,
            int partSize)
        {
            _md5 = md5;
            _fileName = fileName;
            _image = image;
            _length = length;
            _partCount = partCount;
            _partSize = partSize;
        }

        public string MD5
        {
            get { return _md5; }
            set { _md5 = value; }
        }

        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public long PartCount
        {
            get { return _partCount; }
            set { _partCount = value; }
        }

        public int PartSize
        {
            get { return _partSize; }
            set { _partSize = value; }
        }
    }
}
