using System;
using System.Drawing;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap image = new Bitmap($@"C:\Users\Omen\Documents\input_image.jpg");

            Func<Bitmap, Bitmap> reduceFunc = (Bitmap input) =>
            {
                Bitmap output = new Bitmap(input, new Size(input.Width / 2, input.Height / 2));
                return output;
            };

            Func<Bitmap, Bitmap> colorFunc = (Bitmap input) =>
            {
                Bitmap output = new Bitmap(input.Width, input.Height);

                for (int x = 0; x < input.Width; x++)
                {
                    for (int y = 0; y < input.Height; y++)
                    {
                        Color pixel = input.GetPixel(x, y);

                        int r = 255 - pixel.R;
                        int g = 255 - pixel.G;
                        int b = 255 - pixel.B;

                        output.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }

                return output;
            };

            Action<Bitmap> imageAction = (Bitmap input) =>
            {
                input.Save($@"C:\Users\Omen\Documents\output_image.jpg");
                input.Dispose();
            };

            Bitmap reducedImage = reduceFunc(image);
            Bitmap processedImage = colorFunc(reducedImage);
            imageAction(reducedImage);
            imageAction(processedImage);
        }
    }
}

