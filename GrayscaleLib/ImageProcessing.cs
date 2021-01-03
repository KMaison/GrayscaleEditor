using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace GrayscaleLib
{
    public static class ImageProcessing
    {
        public static Action<long> ImageProcessingFinished;
        public static Stopwatch ImageProcessingTimer { get; private set; }
        public static bool Grayscale(string path)
        {
            if (ImageNotExists(path))
                return false;

            try
            {
                ImageProcessingTimer = ImageProcessingTimer ?? new Stopwatch();
                ImageProcessingTimer.Restart();
                var sourceImage = new Bitmap(path);
                var editedImage = ChangeGrayscale(sourceImage);
                sourceImage.Dispose();
                editedImage.Save(path);
                editedImage.Dispose();
                ImageProcessingTimer.Stop();
                ImageProcessingFinished?.Invoke(ImageProcessingTimer.ElapsedMilliseconds);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Bitmap ChangeGrayscale(Bitmap original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);
            using (var g = Graphics.FromImage(newBitmap))
            {
                var colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                    new float[] {0.21f, 0, 0, 0, 0},
                    new float[] {0, 0.72f, 0, 0, 0},
                    new float[] {0, 0, 0.07f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] { 0, 0, 0, 0, 1 }
                    });

                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }

            return newBitmap;
        }

        public static Task GrayscaleAsync(string path)
        {
            return Task.Run(() =>
            {
                Grayscale(path);
            });
        }

        public static bool ImageNotExists(string path)
        {
            return (string.IsNullOrEmpty(path) || !File.Exists(path));
        }
    }
}
