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
        /// <summary>
        /// Преобразование байтовой карты высот в изображение
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Bitmap ToHeightImage(this Map map)
        {
            Bitmap image = new Bitmap(map.SizeX, map.SizeY);
            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    var color = map.HeightMap[x, y];
                    image.SetPixel(x, y, Color.FromArgb(color, color, color));
                }
            }
            return image;
        }

        /// <summary>
        /// Преобразование пиктельной карты цветов в изображение
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
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
