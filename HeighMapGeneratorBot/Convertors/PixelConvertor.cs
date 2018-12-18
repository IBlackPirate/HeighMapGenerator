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
            for (int i = 0; i < sizeX * sizeY * 3; i += 3)
            {
                res[i/3] = new Pixel(arr[i], arr[i + 1], arr[i + 2]);
            }
            return res;
        }

        public static byte[] ToByte(this Pixel[] arr)
        {
            var res = new byte[arr.Length*3];
            for (int i = 0; i < arr.Length*3; i += 3)
            {
                res[i] = (byte)arr[i/3].R;
                res[i + 1] = (byte)arr[i/3].G;
                res[i + 2] = (byte)arr[i/3].B;
            }
            return res;
        }
    }
}
