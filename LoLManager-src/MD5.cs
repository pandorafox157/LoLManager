using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace LoLManager
{
    public struct MD5Calculate
    {
        public static string Get(string FileName)
        {
            FileStream File = new FileStream(FileName, FileMode.Open);
            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] RetVal = MD5.ComputeHash(File);
            File.Close();
            return System.BitConverter.ToString(RetVal).Replace("-", "");
        }
    }
}
