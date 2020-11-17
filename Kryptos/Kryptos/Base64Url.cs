using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Kryptos
{
    public class Base64Url
    {
        private static readonly char base64PadCharacter = '=';
        private static readonly string doubleBase64PadCharacter = "==";
        private static readonly char base64Character62 = '+';
        private static readonly char base64Character63 = '/';
        private static readonly char base64UrlCharacter62 = '-';
        private static readonly char base64UrlCharacter63 = '_';
        public static string Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text))
                .Split(base64PadCharacter)[0]
                .Replace(base64Character62, base64UrlCharacter62)
                .Replace(base64Character63, base64UrlCharacter63);
        }
        public static string Decode(string text)
        {
            text = text.Replace(base64UrlCharacter62, base64Character62)
                .Replace(base64UrlCharacter63, base64Character63);

            text += (text.Length % 4) switch
            {
                0 => string.Empty,
                2 => doubleBase64PadCharacter,
                3 => base64PadCharacter,
                _ => throw new FormatException("Base64Url encoded string is not good."),
            };

            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}
