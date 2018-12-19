using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class DiamondSquareGenerator : IGenerator
    {
        // Шерховатость карты
        public readonly double Roughness;

        private readonly Random random;
        private Map map;
        private bool isRandomBorder;
        private byte defaultBorderValue;

        /// <summary>
        /// Инициализация генератора
        /// </summary>
        /// <param name="roughness">шерховатость, влияет на перепады высот</param>
        /// <param name="seed">сид - влияет на генерацию случайных чисел</param>
        /// <param name="map">карта, по которой будет происходить генерация</param>
        /// <param name="isRandomBorder">будет ли высота граничных точек определяться случайно</param>
        /// <param name="defaultBorderValue">значение высоты граничных точек по умолчанию</param>
        /// <param name="leftTop">высота левого верхнего угла</param>
        /// <param name="leftBottom">высота левого нижнего угла</param>
        /// <param name="rightTop">высота правого верхнего угла</param>
        /// <param name="rightBottom">высота правого нижнего угла</param>
        public DiamondSquareGenerator(double roughness, int seed, Map map, bool isRandomBorder,
            byte defaultBorderValue, byte leftTop, byte leftBottom, byte rightTop, byte rightBottom)
                    : this(roughness, seed, map, isRandomBorder, defaultBorderValue = 0)
        {
            this.map.SetCornerHeight(leftTop, leftBottom, rightTop, rightBottom);
        }

        /// <summary>
        /// Инициализация генератора
        /// </summary>
        /// <param name="roughness">шерховатость, влияет на перепады высот</param>
        /// <param name="seed">сид - влияет на генерацию случайных чисел</param>
        /// <param name="map">карта, по которой будет происходить генерация</param>
        /// <param name="isRandomBorder">будет ли высота граничных точек определяться случайно</param>
        /// <param name="defaultBorderValue">значение высоты граничных точек по умолчанию</param>
        public DiamondSquareGenerator(double roughness, int seed, Map map, bool isRandomBorder, byte defaultBorderValue = 0)
        {
            Roughness = roughness;
            random = new Random(seed);
            this.map = map;
            this.isRandomBorder = isRandomBorder;
            this.defaultBorderValue = defaultBorderValue;
        }

        /// <summary>
        /// Генерирует карту высот
        /// </summary>
        /// <returns>Карта с хранимыми внутри высотами</returns>
        public Map GenerateMap()
        {
            int len = map.SizeX - map.SizeX % 2;
            while (len > 1)
            {
                PreformSquare(len);
                PerformDiamond(len);
                len /= 2;
            }
            return map;
        }

        // Выполнение шага Diamond алгоритма для всей карты
        private void PerformDiamond(int len)
        {
            for (int x = 0; x < map.SizeX - 1; x += len)
            {
                for (int y = 0; y < map.SizeX - 1; y += len)
                {
                    DiamondStep(x, y + len / 2, len / 2);
                    DiamondStep(x + len / 2, y, len / 2);
                    DiamondStep(x + len, y + len / 2, len / 2);
                    DiamondStep(x + len / 2, y + len, len / 2);
                }
            }
        }

        // Выполнение шага Square алгоритма для всей карты
        private void PreformSquare(int len)
        {
            for (int x = 0; x < map.SizeX - 1; x += len)
                for (int y = 0; y < map.SizeX - 1; y += len)
                    SquareStep(x, y, x + len, y + len);
        }

        // Определение высоты средней точки для квадрата, заданного двумя противоположными точками
        private void SquareStep(int leftX, int bottomY, int rightX, int topY)
        {
            // Берем значения высоты во всех вершинах квадрата и суммируем
            var leftTop = map.HeightMap[leftX, topY];
            var leftBottom = map.HeightMap[leftX, bottomY];
            var rightTop = map.HeightMap[rightX, topY];
            var rightBottom = map.HeightMap[rightX, bottomY];
            var sum = leftTop + leftBottom + rightTop + rightBottom;

            var length = (rightX - leftX) / 2;
            var centerX = leftX + length;
            var centerY = bottomY + length;

            // Определяем высоту средней точки
            SetHeight(sum, length, centerX, centerY);
        }

        // В зависимости от настроек, возвращаем либо константу
        // либо случайное число
        private byte GetBorderValue()
        {
            if (isRandomBorder)
                return (byte)(random.Next() % 255);
            return defaultBorderValue;
        }

        // Шаг Diamond для конкретной точки.
        // Определение высоты средней точки в получившихся
        // на шаге Square ромбах
        public void DiamondStep(int centerX, int centerY, int length)
        {
            // Получаем начальные значения высоты на граничных точках
            byte left = GetBorderValue();
            byte right = GetBorderValue();
            byte top = GetBorderValue();
            byte bottom = GetBorderValue();

            // Если точки не выходят за границы массива, берем их высоту из карты
            if (centerX - length >= 0)
                left = map.HeightMap[centerX - length, centerY];
            if (centerX + length < map.SizeX)
                right = map.HeightMap[centerX + length, centerY];
            if (centerY - length >= 0)
                bottom = map.HeightMap[centerX, centerY - length];
            if (centerY + length < map.SizeX)
                top = map.HeightMap[centerX, centerY + length];

            // Определяем высоту средней точки
            var sum = left + right + top + bottom;
            SetHeight(sum, length, centerX, centerY);
        }

        // Возвращает высоту определенной точки, отталкиваясь от суммы соседних и длины текущего шага
        private void SetHeight(int sum, int length, int posX, int posY)
        {
            var result = sum / 4 + random.Next((int)(-Roughness * length), (int)(Roughness * length));
            map.HeightMap[posX, posY] = (byte)(result > 0 ? result % 255: 0);
        }
    }
}