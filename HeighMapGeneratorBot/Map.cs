using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    public class Map
    {
        public readonly byte[,] heightMap;
        public readonly byte[,] colorMap;

        
    }

    public static class Extentions
    {
        public static byte[] ToArray(this byte[,] arr)
        {
            var len = arr.GetLength(0);
            var weight = arr.GetLength(1);

            var res = new List<byte>();

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
