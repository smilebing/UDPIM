using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CSharpWin
{
    public class BufferHelper
    {
        public static byte[] Serialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, obj);
            byte[] datas = stream.ToArray();
            stream.Dispose();
            return datas;
        }

        public static object Deserialize(byte[] datas, int index)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(datas, index, datas.Length - index);
            object obj = bf.Deserialize(stream);
            stream.Dispose();
            return obj;
        }
    }
}
