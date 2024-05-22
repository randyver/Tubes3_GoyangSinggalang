using System;
using Bogus;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cli
{
    public class Cli
    {
        public static void Run()
        {
            Image<Rgba32> image = Image.Load<Rgba32>("../test/1__M_Left_index_finger.BMP");
            Image<Rgba32> image2 = Image.Load<Rgba32>("../test/1__M_Left_index_finger.BMP");
            Image<Rgba32> image3 = Lib.ImageConverter.GetCenteredImage(image2, 30);
            string foo1 = Lib.ImageConverter.ConvertToBinary(image);
            string foo2 = Lib.ImageConverter.ConvertToAscii8Bits(image);
            string foo3 = Lib.ImageConverter.ConvertToAscii8Bits(image3);

            Console.WriteLine("===");
            Console.WriteLine(foo1);
            Console.WriteLine("===");
            Console.WriteLine(foo2);
            Console.WriteLine("===");
            Console.WriteLine(foo3);
            Console.WriteLine("===");

        }
    }

}