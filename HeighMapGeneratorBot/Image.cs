using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace HeighMapGeneratorBot
{
    public static class ImageConvertor
    {
        public static Bitmap ToImage(this byte[] imageData)
        {
            MemoryStream ms = new MemoryStream(imageData);
            var result = new Bitmap(Image.FromStream(ms));
            return result;
        }
    }
}
