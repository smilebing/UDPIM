using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CSharpWin
{


    public static class MD5Helper
    {
        public static string CretaeMD5(string fileName)
        {
            string hashStr = string.Empty;
            try
            {
                FileStream fs = new FileStream(
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(fs);
                hashStr = ByteArrayToHexString(hash);
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hashStr;
        }

        public static string CretaeMD5(Stream stream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(stream);
            return ByteArrayToHexString(hash);
        }

        public static string CretaeMD5(byte[] buffer, int offset, int count)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(buffer, offset, count);
            return ByteArrayToHexString(hash);
        }

        private static string ByteArrayToHexString(byte[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.AppendFormat("{0:X2}", value);
            }
            return sb.ToString();
        }
    }
}
