using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace HeighMapGeneratorBot
{
    static class ImageConvertor
    {
        public static Bitmap ToHeighImage(this byte[] imageData)
        {
            MemoryStream ms = new MemoryStream(imageData);
            var result = new Bitmap(Image.FromStream(ms));
            return result;
        }

        public static Bitmap ToColorImage(this Pixel[,] pixels, int sizeX, int sizeY)
        {
            var res = new Bitmap(sizeX, sizeY);

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    res.SetPixel(i, j, Color.FromArgb(pixels[i, j].R, pixels[i, j].G, pixels[i, j].B));
                }
            }
            return res;
        }
    }
}
