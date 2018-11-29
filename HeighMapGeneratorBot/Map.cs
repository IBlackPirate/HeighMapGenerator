using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    public class Map
    {
        // Ширина карты
        public readonly int SizeX;
        // Высота карты
        public readonly int SizeY;

        public readonly byte[,] HeightMap;
        public readonly byte[,] ColorMap;

        public Map(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            HeightMap = new byte[SizeX, SizeY];
        }

        /// <summary>
        /// Инициализация краев карты заданными значениями
        /// </summary>
        /// <param name="leftTop">Левый верзний угол</param>
        /// <param name="leftBottom">Левый нижний угол</param>
        /// <param name="rightTop">Правый верзний угол</param>
        /// <param name="rightBottom">Правый нижний угол</param>
        public void InitializeMapWithValue(byte leftTop, byte leftBottom, byte rightTop, byte rightBottom)
        {
            HeightMap[0, 0] = leftTop;
            HeightMap[0, SizeY - 1] = leftBottom;
            HeightMap[SizeX - 1, 0] = rightTop;
            HeightMap[SizeX - 1, SizeY - 1] = rightBottom;
        }
    }

    public static class Extentions
    {
        public static T[] ToArray<T>(this T[,] arr)
        {
            var len = arr.GetLength(0);
            var weight = arr.GetLength(1);

            var res = new List<T>();

            for(int i = 0; i < len; i++)
            {
                for(int j = 0; j < weight; j++)
                {
                    res.Add(arr[i, j]);
                }
            }
            return res.ToArray();
        }
    }
}
