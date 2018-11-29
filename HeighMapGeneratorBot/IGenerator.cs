using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    public interface IGenerator
    {
        /// <summary>
        /// Вызывает процесс генерации карты
        /// </summary>
        /// <returns>Массив с высотами каждого пикселя</returns>
        float[,] GenerateMap();
    }
}
