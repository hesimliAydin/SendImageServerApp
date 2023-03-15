using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendImageServer.Helpers
{
    public class ImageHelper
    {
        public static string SaveAndGetImagePath(byte[] buffer)
        {
            ImageConverter ic = new ImageConverter();
            var data = ic.ConvertFrom(buffer);

            Image img = data as Image;
            if (img != null)
            {
                Bitmap bitmap1 = new Bitmap(img);

                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Images";
                Directory.CreateDirectory(path);
                Random random = new Random();
                var N = random.Next(1, 10000000);
                path = path + $"\\image{N}.png";
                bitmap1.Save(path);
                var imagepath = path;
                return imagepath;
            }
            else
            {
                return String.Empty;
            }

        }
    }
}
