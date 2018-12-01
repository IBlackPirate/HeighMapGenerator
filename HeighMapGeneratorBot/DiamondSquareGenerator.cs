using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class DiamondSquareGenerator : IGenerator
    {
        // Шерховатость
        public readonly int Roughness;

        private readonly Random random;
        private Map map;

        /// <summary>
        /// Инициализация генератора
        /// </summary>
        public DiamondSquareGenerator(int roughness, int seed, Map map,
            byte leftTop, byte leftBottom, byte rightTop, byte rightBottom)
        {
            Roughness = roughness;
            random = new Random(seed);
            this.map = map;
            this.map.InitializeMapWithValue(leftTop, leftBottom, rightTop, rightBottom);
        }

        public Map GenerateMap()
        {
            for (int len = (map.Size - 1) / 2; len > 0; len /= 2)
            {
                for (int x = 0; x < map.Size - 1; x += len)
                {
                    for (int y = 0; y < map.Size - 1; y += len)
                        DiamondSquare(x, y, x + len, y + len);
                }
            }
            return map;
        }

        private void DiamondSquare(int leftX, int bottomY, int rightX, int topY)
        {
            int length = (rightX - leftX) / 2;

            SquareStep(leftX, topY, rightX, bottomY);

            Diamond(leftX, topY - length, length);
            Diamond(rightX, bottomY + length, length);
            Diamond(rightX - length, bottomY, length);
            Diamond(leftX + length, topY, length);
        }

        private void SquareStep(int leftX, int topY, int rightX, int bottomY)
        {
            var leftTop = map.HeightMap[leftX, topY];
            var leftBottom = map.HeightMap[leftX, bottomY];
            var rightTop = map.HeightMap[rightX, topY];
            var rightBottom = map.HeightMap[rightX, bottomY];
            var sum = leftTop + leftBottom + rightTop + rightBottom;

            var length = (rightX - leftX) / 2;
            var centerX = leftX + length;
            var centerY = bottomY + length;

            var result = sum / 4 + random.Next(-Roughness, Roughness);
            //map.HeightMap[centerX, centerY] = (byte)(sum / 4);
            map.HeightMap[centerX, centerY] = (byte)(result > 0 ? result : 0);
            //Console.WriteLine(sum / 4 + random.Next(-Roughness, Roughness));
        }

        public void Diamond(int centerX, int centerY, int length)
        {
            byte left = (byte)(random.Next() % 150);
            byte right = (byte)(random.Next() % 150);
            byte top = (byte)(random.Next() % 150);
            byte bottom = (byte)(random.Next() % 150);

            if (centerX - length >= 0)
                left = map.HeightMap[centerX - length, centerY];
            if (centerX + length < map.Size)
                right = map.HeightMap[centerX + length, centerY];
            if (centerY - length >= 0)
                bottom = map.HeightMap[centerX, centerY - length];
            if (centerY + length < map.SizeY)
                top = map.HeightMap[centerX, centerY + length];

            var sum = left + right + top + bottom;
            var result = sum / 4 + random.Next(-Roughness, Roughness);
            //map.HeightMap[centerX, centerY] = (byte)(sum / 4);
            //map.HeightMap[centerX, centerY] = (byte)Math.Abs(sum / 4 + random.Next(-Roughness, Roughness));
            map.HeightMap[centerX, centerY] = (byte)(result > 0 ? result : 0);
            //Console.WriteLine(sum / 4 + random.Next(-Roughness, Roughness));
        }
    }
}
