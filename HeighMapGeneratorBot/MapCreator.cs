using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class MapCreator
    {
        public int MapSizeX;
        public int MapSizeY;
        public int Seed;
        public float Roughness;
        public bool IsRandomBorder;
        public byte DefaultBorderValue;
        public byte LeftTop;
        public byte RightTop;
        public byte LeftBottom;
        public byte RightBottom;
        public BiomeType Biome;
        public int Smoothness;

        public Map CreateMap()
        {
            return new Map(MapSizeX, MapSizeY);
        }

        public DiamondSquareGenerator CreateGenerator()
        {
            return new DiamondSquareGenerator(Roughness, Seed, CreateMap(), IsRandomBorder,
                DefaultBorderValue, LeftTop, LeftBottom, RightTop, RightBottom);
        }
    }
}
