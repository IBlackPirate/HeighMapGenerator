using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class Map
    {
        public readonly byte[,] HeightMap;
        public readonly Pixel[,] ColorMap;
        public int SizeX, SizeY;

        public Map(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            HeightMap = new byte[SizeX, SizeY];
        }

        public Map(byte[,] heightMap, Pixel[,] colorMap, int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            HeightMap = heightMap;
            ColorMap = colorMap;
        }

        /// <summary>
        /// Инициализация краев карты заданными значениями
        /// </summary>
        /// <param name="leftTop">Левый верзний угол</param>
        /// <param name="leftBottom">Левый нижний угол</param>
        /// <param name="rightTop">Правый верзний угол</param>
        /// <param name="rightBottom">Правый нижний угол</param>
        public void SetCornerHeight(byte leftTop, byte leftBottom, byte rightTop, byte rightBottom)
        {
            HeightMap[0, 0] = leftTop;
            HeightMap[0, SizeY - 1] = leftBottom;
            HeightMap[SizeX - 1, 0] = rightTop;
            HeightMap[SizeX - 1, SizeY - 1] = rightBottom;
        }
    }
}
