using System.Text;

namespace Common.EncodingHelper
{
    public static class UtfEncoding
    {
        public static byte[] StringToUtf8ByteArray(string pXmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
