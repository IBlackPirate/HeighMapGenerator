using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    static class PixelConvertor
    {
        public static Pixel[] ToPixels(this byte[] arr, int sizeX, int sizeY)
        {
            var res = new Pixel[sizeX * sizeY];
            for (int i = 0; i < sizeX * sizeY; i += 3)
            {
                res[i] = new Pixel(arr[i], arr[i + 1], arr[i + 2]);
            }
            return res;
        }

        public static byte[] ToByte(this Pixel[] arr, int sizeX, int sizeY)
        {
            var res = new byte[sizeX * sizeY];
            for (int i = 0; i < sizeX * sizeY-2; i += 3)
            {
                res[i] = (byte)arr[i].R;
                res[i + 1] = (byte)arr[i].G;
                res[i + 2] = (byte)arr[i].B;
            }
            return res;
        }
    }
}
