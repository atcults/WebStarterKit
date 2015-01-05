using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Base;

namespace Common.Extensions
{
    public class ImageUtility
    {
        public const string NoImageData = "data:image/jpeg;base64,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";
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();
                return BytesToBase64(imageBytes);
            }
        }

        public static string BytesToBase64(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0) return "";
            var base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        public static byte[] Base64ToBytes(string base64)
        {
            if (base64.Length == 0) return null;
            var imageBytes = Convert.FromBase64String(base64);
            return imageBytes;
        }

        public static byte[] BmpToBytes(Bitmap bmp)
        {
            // stream to save the bitmap to
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, bmp);

            // read to end
            var bmpBytes = ms.GetBuffer();
            bmp.Dispose();
            ms.Close();

            return bmpBytes;
        }

        public static Bitmap BytesToBmp(byte[] bmpBytes)
        {
            var bf = new BinaryFormatter();
            // copy the bytes to the memory
            var ms = new MemoryStream(bmpBytes);
            return (Bitmap)bf.Deserialize(ms);
        }

        public static byte[] DownloadImage(string imageUrl)
        {
            byte[] bytes;

            try
            {
                // Open a connection
                var webRequest = (HttpWebRequest) WebRequest.Create(imageUrl);

                webRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                webRequest.Referer = "http://www.google.com/";
                webRequest.Timeout = 20000;

                using (var response = webRequest.GetResponse())
                {
                    var stm = response.GetResponseStream();
                    if (stm == null) throw new Exception("Response is null");
                    using (var reader = new BinaryReader(stm))
                    {
                        bytes = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log(LogType.Error, typeof(ImageUtility), "DownloadImage", exception);
                return null;
            }
            return bytes;
        }

        public static byte[] OptimizeWebImage(byte[] srcBytes, int height, int width)
        {
            if (srcBytes == null || srcBytes.Length == 0) return null;

            var img = Image.FromStream(new MemoryStream(srcBytes));

            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(0, 0, 100, 100));
            }

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);

            bmp.Dispose();
            ms.Close();
            img.Dispose();

            return ms.GetBuffer();
        }
    }
}