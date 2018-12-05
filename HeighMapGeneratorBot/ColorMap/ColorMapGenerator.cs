﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    static class ColorMapGenerator
    {
        public static readonly List<Biome> Biomes=InicializeBiomes();
        public static Bitmap Generate(this Map map, int biomeType, int smoothIntensivity = 0)
        {
            var currentBiome = Biomes[biomeType].HeightToColor;
            var result = new Pixel[map.SizeX, map.SizeY];

            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    foreach (var layerMaxHeight in currentBiome.Keys)
                    {
                        if (map.HeightMap[x, y] <= layerMaxHeight)
                        {
                            result[x, y] = currentBiome[layerMaxHeight](map.HeightMap[x, y]);
                            break;
                        }
                    }
                }
            }

            for(int i = 0; i < smoothIntensivity; i++)
            {
                result.SmoothImage(map.SizeX, map.SizeY);
            }

            return result.ToColorImage(map.SizeX, map.SizeY);
        }

        public static List<Biome> InicializeBiomes()
        {
            var ran = new Random();
            var res = new List<Biome>
            {
                new Biome(BiomeType.Пустыня, new Dictionary<byte, Func<byte, Pixel>>{
                    { 50, (heigh) => new Pixel(heigh/5, heigh/5, 51 + heigh / 2 + ran.Next(-5, 5)) },
                    { 160, (heigh) => new Pixel(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 255, (heigh) => new Pixel(heigh / 2 + ran.Next(-7, 7) - 25, heigh / 2 + ran.Next(-7, 7) - 25, heigh / 2 + ran.Next(-7, 7) - 12)} }
                ),
                new Biome(BiomeType.Лес, new Dictionary<byte, Func<byte, Pixel>>{
                    { 75, (heigh) => new Pixel(heigh/5, heigh/5, 50 + heigh / 2 + ran.Next(-10, 10)) },
                    { 85, (heigh) => new Pixel(ran.Next(150, 190) + heigh / 2 - 90, ran.Next(150, 190) + heigh / 2 - 90, ran.Next(50, 75) + heigh / 2 - 50) },
                    { 190, (heigh) => new Pixel(0, heigh/2 + 30 + ran.Next(-15, 15), 0) },
                    { 255, (heigh) => new Pixel(heigh / 2 + ran.Next(-3, 3) - 10, heigh / 2 + ran.Next(-3, 3) - 10, heigh / 2 + ran.Next(-3, 3) - 5)} }
                )
            };
            return res;
        }

        public static void SmoothImage(this Pixel[,] pixels, int sizeX, int sizeY)
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