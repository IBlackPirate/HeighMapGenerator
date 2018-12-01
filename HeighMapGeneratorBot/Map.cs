using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    public class Map
    {
        // Ширина и высота карты
        public readonly int Size;

        public readonly byte[,] HeightMap;

        ////////////////////////////////
        public readonly byte[,] ColorMap;
        public int SizeX, SizeY;
        ////////////////////////////////

        public Map(int size)
        {
            Size = size;
            HeightMap = new byte[Size, Size];
        }

        public Map(byte[,] heightMap, byte[,] colorMap, int size, int deleteMe = 0)
        {
            Size = size;
            HeightMap = heightMap;
            ColorMap = colorMap;
        }

        public void Realize()
        {
            for(int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    var d = HeightMap[x, y] / 255d;
                    d *= d;
                    HeightMap[x, y] = (byte)(d * 255 * 255);
                }
            }
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
            HeightMap[0, Size - 1] = leftBottom;
            HeightMap[Size - 1, 0] = rightTop;
            HeightMap[Size - 1, Size - 1] = rightBottom;
        }

        public Bitmap ToHeightBitmap()
        {
            Bitmap image = new Bitmap(Size, Size);
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    var color = HeightMap[x, y];
                    image.SetPixel(x, y, Color.FromArgb(color, color, color));
                }
            }
            return image;
        }
    }

    public static class Extentions
    {
        public static T[] ToArray<T>(this T[,] arr)
        {
            var len = arr.GetLength(0);
            var weight = arr.GetLength(1);

            var res = new List<T>();

            for (int i = 0; i < weight; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    res.Add(arr[i, j]);
                }
            }
            return res.ToArray();
        }

        public static T[,] ToMatrix<T>(this T[] arr, int sizeX, int sizeY)
        {
            var res = new T[sizeX, sizeY];

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    res[i, j] = arr[sizeX * i + j];
                }
            }
            return res;
        }
    }
}
