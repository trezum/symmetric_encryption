using System;
using System.Linq;
using System.Text;

namespace symmetric_encryption
{
    public static class ExtensionMethods
    {
        public static string ToHexString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            for (int i = 0; i < ba.Length; i++)
            {
                if (i % 16 == 0 && i > 0)
                {
                    hex.Append(" ");
                }
                hex.AppendFormat("{0:x2}", ba[i]);
            }
            return hex.ToString();
        }

        public static byte[] ToByteArray(this string hex)
        {
            var removedSpaces = hex.Replace(" ", string.Empty);
            return Enumerable.Range(0, removedSpaces.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(removedSpaces.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}