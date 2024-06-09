using Org.BouncyCastle.Crypto;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using ZstdSharp.Unsafe;

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

        public static Image<Rgba32> GetCenteredImage(Image<Rgba32> image, int size, int top)
        {
            // If the image is smaller than the requested size, throw an exception
            if (image.Width < size || image.Height < size)
            {
                                Console.WriteLine(size);
                Console.WriteLine(image.Width);
                Console.WriteLine(image.Height);
                throw new ArgumentException("Image size is smaller than the requested size");
            }

            int x = (image.Width - size) / 2;

            // Crop the image to the center
            return image.Clone(ctx => ctx.Crop(new Rectangle(x, top, size, size)));
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

                /* Convert image to a ASCII 8 bits representation */
        public static char[,] ConvertToAscii8BitsArray(Image<Rgba32> image)
        {
            // Image to ASCII 8 bits
            char[,] ascii8Bits = new char[image.Height, image.Width / 8 + 1];

            for (int y = 0; y < image.Height; y++)
            {
                string binaryTemp = "";
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    binaryTemp += grayScale > 127 ? "1" : "0";
                    if (binaryTemp.Length == 8)
                    {
                        ascii8Bits[y, x / 8] = (char)Convert.ToInt32(binaryTemp, 2);
                        binaryTemp = "";
                    }
                }

                // If the last binaryTemp is not empty, convert it to a char
                if (binaryTemp.Length > 0)
                {
                    ascii8Bits[y, image.Width / 8] = (char)Convert.ToInt32(binaryTemp.PadRight(8, '0'), 2);
                    binaryTemp = "";
                }
            }

            return ascii8Bits;
        }

        public static Image<Rgba32> OmitWhiteSpace (Image<Rgba32> image) {
            // Omit whitespaces in the side
            int x_left_eff = 4, x_right_eff = image.Width - 5;
            int y_top_eff = 4, y_bottom_eff = image.Height - 5;

            // Find the first non-white pixel in the x-axis
            
            // get the y_top_eff
            int count = 0;
            double rasio = Math.Round(image.Width*image.Height/(double)10000);
            int limit = (int) Math.Floor(10*rasio);
            for (int y = 4; y < image.Height - 4 && count < limit; y++)
            {
                for (int x = 4; x < image.Width - 4; x++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    int binary = grayScale > 127 ? 1 : 0;

                    if (binary == 0)
                    {
                        count++;
                        if (count >= limit)
                        {
                            y_top_eff = y;
                            break;
                        }
                    }
                }
            }

            // get the y_bottom_eff
            count = 0;
            for (int y = image.Height - 5; y >= 4 && count < limit; y--)
            {
                for (int x = 4; x < image.Width - 4; x++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    int binary = grayScale > 127 ? 1 : 0;

                    if (binary == 0)
                    {
                        count++;
                        if (count >= limit)
                        {
                            y_bottom_eff = y;
                            break;
                        }
                    }
                }
            }

            // get the x_left_eff
            count = 0;
            for (int x = 4; x < image.Width - 4 && count < limit; x++)
            {
                for (int y = 4; y < image.Height - 4; y++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    int binary = grayScale > 127 ? 1 : 0;

                    if (binary == 0)
                    {
                        count++;
                        if (count >= limit)
                        {
                            x_left_eff = x;
                            break;
                        }
                    }
                }
            }

            // get the x_right_eff
            count = 0;
            for (int x = image.Width - 5; x >= 4 && count < limit; x--)
            {
                for (int y = 4; y < image.Height - 4; y++)
                {
                    Rgba32 pixel = image[x, y];
                    int grayScale = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    int binary = grayScale > 127 ? 1 : 0;

                    if (binary == 0)
                    {
                        count++;
                        if (count >= limit)
                        {
                            x_right_eff = x;
                            break;
                        }
                    }
                }
            }

            // Crop the image to the effective area
            // Minimum width is 40 and height is 40

            // if (x_right_eff - x_left_eff < 40)
            // {
            //     int try_x_left_eff = Math.Max(0, x_left_eff - (40 - (x_right_eff - x_left_eff)) / 2);
            //     int try_x_right_eff = Math.Min(image.Width - 1, x_right_eff + (40 - (x_right_eff - x_left_eff)) / 2);

            //     if (try_x_left_eff == 0) {
            //         x_left_eff = 0;
            //         x_right_eff = 40;
            //     } else if (try_x_right_eff == image.Width - 1) {
            //         x_right_eff = image.Width - 1;
            //         x_left_eff = image.Width - 41;
            //     } else {
            //         x_left_eff = try_x_left_eff;
            //         x_right_eff = try_x_right_eff;
            //     }
            // }


            // if (y_bottom_eff - y_top_eff < 40)
            // {
            //     int try_y_top_eff = Math.Max(0, y_top_eff - (40 - (y_bottom_eff - y_top_eff)) / 2);
            //     int try_y_bottom_eff = Math.Min(image.Height - 1, y_bottom_eff + (40 - (y_bottom_eff - y_top_eff)) / 2);

            //     if (try_y_top_eff == 0) {
            //         y_top_eff = 0;
            //         y_bottom_eff = 40;
            //     } else if (try_y_bottom_eff == image.Height - 1) {
            //         y_bottom_eff = image.Height - 1;
            //         y_top_eff = image.Height - 41;
            //     } else {
            //         y_top_eff = try_y_top_eff;
            //         y_bottom_eff = try_y_bottom_eff;
            //     }
            // }




            return image.Clone(ctx => ctx.Crop(new Rectangle(x_left_eff, y_top_eff, x_right_eff - x_left_eff + 1, y_bottom_eff - y_top_eff + 1)));
        }

        public static Image<Rgba32>[] GetThreeCenteredImage(Image<Rgba32> image, int size)
        {
            Image<Rgba32>[] images = new Image<Rgba32>[3];
            for (int i = 0; i < 3; i++)
            {
                int top = Math.Max(image.Height /2 * i - size, 0);
                images[i] = GetCenteredImage(image, size, top);
            }
            return images;
        }

        public static string[] GetCenteredArrays(string originalArray) {
            string[] ret = new string[5];
            for (int i = 0; i < 5; i++) {
                ret[i] = "";
            }
            
            int count = 0;
            for (int i = 20; i < 40; ++i) {
                ret[0] += originalArray[i];
                if (originalArray[i] == (char) 255) {
                    count++;
                }
            }

            for (int i = originalArray.Length / 2 - 5; i < originalArray.Length / 2 + 5; ++i) {
                ret[1] += originalArray[i];
            }

            for (int i = originalArray.Length - 20; i >= originalArray.Length - 40; --i) {
                ret[2] += originalArray[i];
            }

            for (int i = originalArray.Length/2 - 20; i < originalArray.Length/2 - 10; ++i) {
                ret[3] += originalArray[i];
            }

            for (int i = originalArray.Length/2 + 10; i < originalArray.Length/2 + 20; ++i) {
                ret[4] += originalArray[i];
            }

            return ret;
        }
    }
}