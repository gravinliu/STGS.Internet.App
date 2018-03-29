using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace STGS.Internet.Tool
{

    public class Encryption
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MD5Hash(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
