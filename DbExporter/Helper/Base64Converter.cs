using System;

namespace DbExporter.Helper
{
    public class Base64Converter
    {
        public static byte[] Base64ToImage(string base64)
        {
            if (string.IsNullOrEmpty(base64))
            {
                return null;
            }
            return Convert.FromBase64String(base64);
        }

        public static string ImageToBase64(byte[] bitmap)
        {
            if (bitmap == null)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(bitmap);
        }
    }
}
