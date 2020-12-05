namespace Kryptos
{
    public class Jwt
    {
        public static string Decode(string text)
        {
            var parts = text.Split('.');
            var header = Base64Url.Decode(parts[0]);
            var payload = Base64Url.Decode(parts[1]);
            var signature = $"{{{parts[2]}}}";
            return header + "." + payload + "." + signature;
        }
    }
}
