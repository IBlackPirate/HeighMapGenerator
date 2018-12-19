using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    static class ArrayConvertor
    {
        /// <summary>
        /// Конвертация из двухмерного массива в одномерный
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Конвертация из одномерного массива в двухмерный
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
        public static T[,] ToMatrix<T>(this T[] arr, int sizeX, int sizeY)
        {
            var res = new T[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    res[i, j] = arr[sizeY * i + j];
                }
            }
            return res;
        }
    }
}
