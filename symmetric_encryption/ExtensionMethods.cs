﻿using System.Text;

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
    }
}