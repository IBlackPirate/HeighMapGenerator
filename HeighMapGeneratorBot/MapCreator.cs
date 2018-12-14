using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class MapCreator
    {
        public static int MapSizeX;
        public static int MapSizeY;
        public static int Seed;
        public static float Roughness;
        public static bool IsRandomBorder;
        public static byte DefaultBorderValue;
        public static byte LeftTop;
        public static byte RightTop;
        public static byte LeftBottom;
        public static byte RightBottom;
        public static BiomeType Biome;
        public static int Smoothness;

        public static Map CreateMap()
        {
            return new Map(MapSizeX, MapSizeY);
        }

        public static DiamondSquareGenerator CreateGenerator()
        {
            return new DiamondSquareGenerator(Roughness, Seed, CreateMap(), IsRandomBorder,
                DefaultBorderValue, LeftTop, LeftBottom, RightTop, RightBottom);
        }
    }
}
