using System;
using System.Collections.Generic;
using System.Text;

namespace STGS.Internet.Tool
{
    public static class StringExtensions
    {
        /// <summary>
        /// 返回错误Code
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static int GetCode(this string Str)
        {
            if (!string.IsNullOrWhiteSpace(Str))
            {
                return int.Parse(Str.Substring(1));
            }
            return 10000;
        }
    }
}
