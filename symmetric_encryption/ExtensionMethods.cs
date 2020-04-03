using System;
using System.Linq;
using System.Text;

namespace symmetric_encryption
{
    public static class ExtensionMethods
    {
        public static string ToHexString(this byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (i % 16 == 0 && i > 0)
                {
                    hex.Append(" ");
                }
                hex.AppendFormat("{0:x2}", byteArray[i]);
            }
            return hex.ToString();
        }

        public static byte[] FromHexToByteArray(this string hex)
        {
            var removedSpaces = hex.Replace(" ", string.Empty);
            return Enumerable.Range(0, removedSpaces.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(removedSpaces.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToBase64String(this byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }

        public static byte[] FromBase64ToByteArray(this string base64String)
        {            
            return Convert.FromBase64String(base64String);
        }
    }
}