using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KDKmusicWebsite.Areas.Admin.Extensions
{
    public static class EncryptPassword
    {
        #region MD5 Encrypt
        public static string mahoamd5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var dulieu = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();

                for (int i = 0; i < dulieu.Length; i++)
                {
                    builder.Append(dulieu[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        #endregion
    }
}