using System.IO;

namespace Ademund.Utils.StreamExtensions
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return (memoryStream).ToArray();

            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        public static string ToStringWithEncoding(this Stream stream, System.Text.Encoding encoding = null)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, encoding ?? System.Text.Encoding.UTF8))
            {
                if (stream.CanSeek)
                {
                    long initialPosition = stream.Position;
                    stream.Position = 0;
                    string content = reader.ReadToEnd();
                    stream.Position = initialPosition;
                    return content;
                }
                return string.Empty;
            }
        }
    }
}