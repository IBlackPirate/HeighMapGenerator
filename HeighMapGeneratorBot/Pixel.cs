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
}
