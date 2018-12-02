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
            int len = map.Size - map.Size % 2;
            while (len > 1)
            {
                PreformSquare(len);
                PerformDiamond(len);
                len /= 2;
            }
            return map;
        }

        private void PerformDiamond(int len)
        {
            for (int x = 0; x < map.Size - 1; x += len)
            {
                for (int y = 0; y < map.Size - 1; y += len)
                {
                    DiamondStep(x, y + len / 2, len / 2);
                    DiamondStep(x + len / 2, y, len / 2);
                    DiamondStep(x + len, y + len / 2, len / 2);
                    DiamondStep(x + len / 2, y + len, len / 2);
                }
            }
        }

        private void PreformSquare(int len)
        {
            for (int x = 0; x < map.Size - 1; x += len)
                for (int y = 0; y < map.Size - 1; y += len)
                    SquareStep(x, y, x + len, y + len);
        }

        private void SquareStep(int leftX, int bottomY, int rightX, int topY)
        {
            var leftTop = map.HeightMap[leftX, topY];
            var leftBottom = map.HeightMap[leftX, bottomY];
            var rightTop = map.HeightMap[rightX, topY];
            var rightBottom = map.HeightMap[rightX, bottomY];
            var sum = leftTop + leftBottom + rightTop + rightBottom;

            var length = (rightX - leftX) / 2;
            var centerX = leftX + length;
            var centerY = bottomY + length;

            SetHeight(sum, length, centerX, centerY);
        }

        public void DiamondStep(int centerX, int centerY, int length)
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
            SetHeight(sum, length, centerX, centerY);
        }

        private void SetHeight(int sum, int length, int posX, int posY)
        {
            var result = sum / 4 + random.Next(-Roughness * length, Roughness * length);
            map.HeightMap[posX, posY] = (byte)(result > 0 ? result % 255 : 0);
        }
    }
}