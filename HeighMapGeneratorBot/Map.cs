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
        /// <summary>
        /// Карта высоты в виде матрицы байт.
        /// Значение определенной точки - ее высота
        /// </summary>
        public readonly byte[,] HeightMap;

        /// <summary>
        /// Карта цвета, образованная по карте высот
        /// </summary>
        public readonly Pixel[,] ColorMap;

        // Размеры карты
        public int SizeX, SizeY;

        /// <summary>
        /// Инициализация карты по размерам
        /// </summary>
        /// <param name="sizeX">Длина</param>
        /// <param name="sizeY">Ширина</param>
        public Map(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            HeightMap = new byte[SizeX, SizeY];
            ColorMap = new Pixel[SizeX, SizeY];
        }

        /// <summary>
        /// Инициализация карты по заданной карте высот и карте цвета
        /// </summary>
        /// <param name="heightMap">Карты высот</param>
        /// <param name="colorMap">Карта цвета</param>
        /// <param name="sizeX">Длина</param>
        /// <param name="sizeY">Ширина</param>
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
        /// <param name="rightTop">Правый верхний угол</param>
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
