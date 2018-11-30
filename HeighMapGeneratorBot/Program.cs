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

            var c = new Bitmap(40, 40);

            for(int i = 0; i < 40; i++)
            {
                for(int j = 0; j < 40; j++)
                {
                    if (i < 20 && j < 20)
                    {
                        c.SetPixel(i, j, System.Drawing.Color.Black);
                    }
                    if (i < 20 && j >= 20)
                    {
                        c.SetPixel(i, j, System.Drawing.Color.Red);
                    }
                    if (i >= 20 && j < 20)
                    {
                        c.SetPixel(i, j, System.Drawing.Color.Blue);
                    }
                    else
                    {
                        c.SetPixel(i, j, System.Drawing.Color.White);
                    }

                }
            }



            c.Save(@"C:\result_new.png", System.Drawing.Imaging.ImageFormat.Png);

            byte[] imageData = null;
            FileInfo fInfo = new FileInfo(@"C:\result_new.png");
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(@"C:\result_new.png", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            imageData = br.ReadBytes((int)numBytes);
        }
    }
}
