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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Шаг 'Square' в алгоритме
        /// Нахождение высоты средней точки
        /// </summary>
        private void Square(int leftX, int topY, int rightX, int bottomY)
        {
            var leftTop = map.HeightMap[leftX, topY];
            var leftBottom = map.HeightMap[leftX, bottomY];
            var rightTop = map.HeightMap[rightX, topY];
            var rightBottom = map.HeightMap[rightX, bottomY];
            var sum = leftTop + leftBottom + rightTop + rightBottom;

            var centerX = leftX + (rightX - leftX) / 2;
            var centerY = bottomY + (topY - bottomY) / 2;

            map.HeightMap[centerX, centerY] = (byte)(sum / 4 + random.Next(-Roughness, Roughness));
        }


        public void Diamond()
        {

        }
    }
}
