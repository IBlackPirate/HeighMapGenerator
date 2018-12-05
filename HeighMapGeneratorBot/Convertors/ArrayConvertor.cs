using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    static class ArrayConvertor
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
