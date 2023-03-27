using System.Text;

namespace pxtools
{
    public static class Extensions
    {
        public static string ToCleanString(this byte[] bytes)
        {
            return new string(bytes.Where(b => b != 0).Select(b => (char)b).ToArray());

            //StringBuilder sb = new StringBuilder();
            //foreach(byte b in bytes)
            //{
            //    if (b == '\0')
            //        return sb.ToString();
            //    sb.Append((char)b);
            //}
            //return sb.ToString();
        }
    }
}
