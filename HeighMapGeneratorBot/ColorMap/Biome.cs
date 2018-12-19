using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HeighMapGeneratorBot
{
    public enum BiomeType
    {
        Пустыня,
        Лес
    }

    class Biome
    {
        // Тип биома
        public readonly BiomeType Type;
        // Список диапазонов высот и соответствующие им функции преобразования высоты в цвет
        public readonly Dictionary<byte, Func<byte, Pixel>> HeightToColor;

        public Biome(BiomeType type, Dictionary<byte, Func<byte, Pixel>> heightToColor)
        {
            Type = type;
            HeightToColor = heightToColor;
        }
    }
}
