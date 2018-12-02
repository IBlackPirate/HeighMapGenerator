using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(1025);
            var generator = new DiamondSquareGenerator(2, new Random().Next(), map, 30 , 0, 10, 135);

            var c = generator.GenerateMap().ToHeightBitmap();
            var r = ColorMapGenerator.Generate(1, map.HeightMap, map.Size);
            r.Save(@"C:\result_new_normal.png", System.Drawing.Imaging.ImageFormat.Png);
            map.Realize();
            c.Save(@"C:\result_new_by.png", System.Drawing.Imaging.ImageFormat.Png);
            c = generator.GenerateMap().ToHeightBitmap();
            c.Save(@"C:\result_new_real.png", System.Drawing.Imaging.ImageFormat.Png);

            r = ColorMapGenerator.Generate(1, map.HeightMap, map.Size);
            r.Save(@"C:\result_new.png", System.Drawing.Imaging.ImageFormat.Png);

            //byte[] imageData = null;
            //FileInfo fInfo = new FileInfo(@"C:\result_new.png");
            //long numBytes = fInfo.Length;
            //FileStream fStream = new FileStream(@"C:\result_new.png", FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fStream);
            //imageData = br.ReadBytes((int)numBytes);

        }
    }
}
