using System.Text;

namespace Kryptos
{
    public static class Extensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            var stringBuilder = new StringBuilder(bytes.Length);
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }
    }
}
