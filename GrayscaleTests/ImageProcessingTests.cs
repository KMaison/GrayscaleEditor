using System;
using System.IO;
using GrayscaleLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests
{
    [TestClass]
    public class ImageProcessingTests
    {
        [TestMethod]
        public void Grayscale_SuccessfulEditing_ReturnsTrue()
        {
            var correctPath = getPathToImage(true);

            var result = ImageProcessing.Grayscale(correctPath);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Grayscale_EditingFailed_ReturnsFalse()
        {
            var fakePath = getPathToImage(false);

            var result = ImageProcessing.Grayscale(fakePath);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ImageNotExists_NotExistImage_ReturnsTrue()
        {
            var fakePath = getPathToImage(false);

            var result = ImageProcessing.ImageNotExists(fakePath);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ImageNotExists_ExistImage_ReturnsFalse()
        {
            var path = getPathToImage(true);

            var result = ImageProcessing.ImageNotExists(path);

            Assert.IsFalse(result);
        }

        private string getPathToImage(bool exist)
        {
            var filename = exist ? "image.png" : "notExist.png";
            return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"), "FilesToTests", filename);
        }
    }
}
