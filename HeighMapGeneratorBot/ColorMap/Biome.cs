﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HeighMapGeneratorBot
{
    class Biome
    {
        public readonly string Name;
        public readonly Dictionary<byte, Func<byte, Pixel>> HeightToColor;

        public Biome(string name, Dictionary<byte, Func<byte, Pixel>> heightToColor)
        {
            Name = name;
            HeightToColor = heightToColor;
        }
    }
}