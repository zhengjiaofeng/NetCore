﻿using System.Security.Cryptography;
using System.Text;

namespace LLBLL.Common.Tool
{
    public class Encry
    {
    }

    public class Md5Encry
    {

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encry(string str)
        {
            //32位大写
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }
                string result = builder.ToString();
                return result;
            }
        }

    }
}
