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
        public readonly float Roughness;
        // Ширина карты
        public readonly int SizeX;
        // Высота карты
        public readonly int SizeY;

        private readonly Random random;
        private float[,] map;

        /// <summary>
        /// Инициализация генератора
        /// </summary>
        /// <param name="sizeX">Ширина карты</param>
        /// <param name="sizeY">Высота карты</param>
        /// <param name="roughness">Шерховатость</param>
        /// <param name="seed">Параметр, влияющай на выбор случайных чисел</param>
        /// <param name="leftTop">Высота левого верхнего края</param>
        /// <param name="leftBottom">Высота левого нижнего края</param>
        /// <param name="rightTop">Высота правого верхнего края</param>
        /// <param name="rightBottom">Высота правого нижнего края</param>
        public DiamondSquareGenerator(int sizeX, int sizeY, float roughness, int seed,
            float leftTop, float leftBottom, float rightTop, float rightBottom)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Roughness = roughness;
            random = new Random(seed);
            InitializeMap(leftTop, leftBottom, rightTop, rightBottom);
        }

        // Служит для инициализации карты начальными значениями
        private void InitializeMap(float leftTop, float leftBottom, float rightTop, float rightBottom)
        {
            map = new float[SizeX, SizeY];
            map[0, 0] = leftTop;
            map[0, SizeY - 1] = leftBottom;
            map[SizeX - 1, 0] = rightTop;
            map[SizeX - 1, SizeY - 1] = rightBottom;
        }

        /// <summary>
        /// Генерирует карту, используя алгоритм DiamondSquare
        /// </summary>
        /// <returns>Массив с высотами каждого пикселя</returns>
        public float[,] GenerateMap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Шаг 'Square' в алгоритме
        /// Нахождение высоты средней точки
        /// </summary>
        private void Square(float leftX, float leftY, float rightX, float rightY)
        {
            
        }


        public void Diamond()
        {

        }
    }
}
