using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWin
{


    [Serializable]
    public class SendCell : IDataCell
    {
        private int _messageID;
        public int MessageID
        {
            get { return _messageID; }
            set { _messageID = value; }
        }

        private object _data;
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public SendCell() { }

        public SendCell(
            int messageID,
            object data)
        {
            _messageID = messageID;
            _data = data;
        }

        public byte[] ToBuffer()
        {
            byte[] data = BufferHelper.Serialize(_data);
            byte[] id = BitConverter.GetBytes(MessageID);

            byte[] buffer = new byte[data.Length + id.Length];
            Buffer.BlockCopy(id, 0, buffer, 0, id.Length);
            Buffer.BlockCopy(data, 0, buffer, id.Length, data.Length);
            return buffer;
        }

        public void FromBuffer(byte[] buffer)
        {
            _messageID = BitConverter.ToInt32(buffer, 0);
            _data = BufferHelper.Deserialize(buffer, 4);
        }
    }
}
