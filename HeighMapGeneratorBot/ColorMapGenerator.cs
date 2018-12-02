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
        public static Bitmap Generate(int biomeType, byte[,] heighMap, int size)
        {
            var biomes = InicializeBiomes();
            var currentBiome = biomes[biomeType].HeightToColor;
            var res = new Bitmap(size, size);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    foreach (var e in currentBiome.Keys)
                    {
                        if (heighMap[x, y] <= e)
                        {
                            res.SetPixel(x, y, currentBiome[e](heighMap[x, y]));
                            break;
                        }
                    }
                }
            }

            return res;
        }

        public static List<Biome> InicializeBiomes()
        {
            var ran = new Random();
            var res = new List<Biome>
            {
                new Biome("Пустыня", new Dictionary<byte, Func<byte, Color>>{
                    { 50, (heigh) => Color.FromArgb(heigh/5, heigh/5, 51 + heigh / 2 + ran.Next(-5, 5)) },
                    { 160, (heigh) => Color.FromArgb(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 255, (heigh) => Color.FromArgb(heigh  / 2 + ran.Next(-7, 7) - 25, heigh  / 2 + ran.Next(-7, 7) - 25, heigh  / 2 + ran.Next(-7, 7) - 12)} }
                ),
                new Biome("Лес", new Dictionary<byte, Func<byte, Color>>{
                    { 65, (heigh) => Color.FromArgb(heigh/5, heigh/5, 50 + heigh / 2 + ran.Next(-10, 10)) },
                    { 75, (heigh) => Color.FromArgb(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 140, (heigh) => Color.FromArgb(0, heigh/2 + 30 + ran.Next(-15, 15), 0) },
                    { 255, (heigh) => Color.FromArgb(heigh  / 2 + ran.Next(-3, 3) - 10, heigh  / 2 + ran.Next(-3, 3) - 10, heigh  / 2 + ran.Next(-3, 3) - 5)} }
                )
            };
            return res;
        }

        //void SmoothImg(Bitmap bitmap, int size)
        //{
        //    for (int i = 0; i < size; i++)
        //    {
        //        for(int j = 0; j < size; j++)
        //        {
        //            Color clr1 = bitmap.GetPixel(i, j);
        //            Color clr2 = bitmap.GetPixel(i + 1, j);
        //            bitmap.S = (2 * clr1 + clr2) / 3;
        //            colors[i + 1] = (clr1 + 2 * clr2) / 3;
        //        }

        //    }
        //}
    }
}
