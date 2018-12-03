using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    static class ColorMapGenerator
    {
        public static Bitmap Generate(int biomeType, byte[,] heighMap, int sizeX, int sizeY)
        {
            var biomes = InicializeBiomes();
            var currentBiome = biomes[biomeType].HeightToColor;
            var res = new Pixel[sizeX, sizeY];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    foreach (var e in currentBiome.Keys)
                    {
                        if (heighMap[x, y] <= e)
                        {
                            res[x, y] = currentBiome[e](heighMap[x, y]);
                            break;
                        }
                    }
                }
            }
            //res.SmoothImg(sizeX, sizeY);
            //res.SmoothImg(sizeX, sizeY);

            return res.ToColorImage(sizeX, sizeY);
        }

        public static List<Biome> InicializeBiomes()
        {
            var ran = new Random();
            var res = new List<Biome>
            {
                new Biome("Пустыня", new Dictionary<byte, Func<byte, Pixel>>{
                    { 50, (heigh) => new Pixel(heigh/5, heigh/5, 51 + heigh / 2 + ran.Next(-5, 5)) },
                    { 160, (heigh) => new Pixel(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 255, (heigh) => new Pixel(heigh / 2 + ran.Next(-7, 7) - 25, heigh / 2 + ran.Next(-7, 7) - 25, heigh / 2 + ran.Next(-7, 7) - 12)} }
                ),
                new Biome("Лес", new Dictionary<byte, Func<byte, Pixel>>{
                    { 75, (heigh) => new Pixel(heigh/5, heigh/5, 50 + heigh / 2 + ran.Next(-10, 10)) },
                    { 85, (heigh) => new Pixel(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 190, (heigh) => new Pixel(0, heigh/2 + 30 + ran.Next(-15, 15), 0) },
                    { 255, (heigh) => new Pixel(heigh / 2 + ran.Next(-3, 3) - 10, heigh / 2 + ran.Next(-3, 3) - 10, heigh / 2 + ran.Next(-3, 3) - 5)} }
                )
            };
            return res;
        }

        public static void SmoothImg(this Pixel[,] pixels, int sizeX, int sizeY)
        {
            for (int i = 0; i < sizeX - 1; i++)
            {
                for (int j = 0; j < sizeY - 1; j++)
                {
                    pixels[i, j] = (pixels[i, j] * 6 + pixels[i + 1, j] + pixels[i, j + 1] + pixels[i + 1, j + 1]) / 9;
                }

            }
        }
    }
}