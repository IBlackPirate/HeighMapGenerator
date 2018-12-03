using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class Pixel
    {
        public int R;
        public int G;
        public int B;

        public Pixel(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        
        public static Pixel operator *(Pixel pixel, int num) => new Pixel(pixel.R*num, pixel.G*num, pixel.B*num);
        public static Pixel operator /(Pixel pixel, int num) => new Pixel(pixel.R / num, pixel.G / num, pixel.B / num);
        public static Pixel operator +(Pixel first, Pixel second) => new Pixel(first.R + second.R, first.G + second.G, first.B + second.B);
    }

    static class PixelExtensions
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
    }

}
