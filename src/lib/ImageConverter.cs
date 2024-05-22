using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;

namespace Lib
{
    public class ImageConverter
    {
        /* Get image center size of size */
        public static Image<Rgba32> GetCenteredImage(Image<Rgba32> image, int size)
        {
            // If the image is smaller than the requested size, throw an exception
            if (image.Width < size || image.Height < size)
            {
                throw new ArgumentException("Image size is smaller than the requested size");
            }

            int x = (image.Width - size) / 2;
            int y = (image.Height - size) / 2;

            // Crop the image to the center
            return image.Clone(ctx => ctx.Crop(new Rectangle(x, y, size, size)));
        }

        /* Convert image to a binary string representation */
        public static string ConvertToBinary(Image<Rgba32> image)
        {
            string binary = "";
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    binary += grayScale > 127 ? "1" : "0";
                }

            }
            return binary;
        }

        /* Convert image to a ASCII 8 bits representation */
        public static string ConvertToAscii8Bits(Image<Rgba32> image)
        {
            string ascii8Bits = "";
            string binaryTemp = "";
            // For every 8 pixel of grayscale , convert it to a char
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    binaryTemp += grayScale > 127 ? "1" : "0";
                    if (binaryTemp.Length == 8)
                    {
                        ascii8Bits += (char)Convert.ToInt32(binaryTemp, 2);
                        binaryTemp = "";
                    }
                }
            }

            // If the last binaryTemp is not empty, convert it to a char
            if (binaryTemp.Length > 0)
            {
                ascii8Bits += (char)Convert.ToInt32(binaryTemp.PadRight(8, '0'), 2);
            }

            return ascii8Bits;
        }
    }
}