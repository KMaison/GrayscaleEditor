using System.Drawing;
using System.Threading.Tasks;

namespace Zadanie1
{
    public static class ImageProcessing
    {
        public static void Grayscale(string path)
        {
            using (var bmp = new Bitmap(path))
            {
                SetPixels(bmp);
                bmp.Save(@"c:\Users\kmaisonx\OneDrive - Intel Corporation\Desktop\edited.jpg");
            }
        }

        public static void GrayscaleAsync(string path)
        {
            Task.Run(() =>
            {
                Grayscale(path);
            });
        }

        private static void SetPixels(Bitmap bmp)
        {
            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    bmp.SetPixel(x, y, Color.FromArgb(pixel.A, (int)(pixel.R * 0.21), (int)(pixel.G * 0.72), (int)(0.07 * pixel.B)));
                }
            }
        }
    }
}
