using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GrayscaleLib;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GrayscaleEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand LoadImage { get; private set; }
        public ICommand ConvertImage { get; private set; }

        private string _imagePath;
        private ImageSource _copyOfOriginalImage;
        private ImageSource _copyOfEditedImage;
        private string _durationOfProcessing;
        private bool _convertingIsSync = true;

        public MainViewModel()
        {
            ImageProcessing.ImageProcessingFinished += OnImageProcessingFinished;
            InitLoadImage();
            InitConvertImage();
        }

        public string DurationOfProcessing
        {
            get => _durationOfProcessing;
            set
            {
                _durationOfProcessing = value;
                RaisePropertyChanged("DurationOfProcessing");
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                CopyOfOriginalImage = GetCopyOfImage(value);
                RaisePropertyChanged("Image");
            }
        }

        public ImageSource CopyOfOriginalImage
        {
            get => _copyOfOriginalImage;
            set
            {
                _copyOfOriginalImage = value;
                RaisePropertyChanged("CopyOfOriginalImage");
            }
        }

        public ImageSource CopyOfEditedImage
        {
            get => _copyOfEditedImage;
            set
            {
                _copyOfEditedImage = value;
                RaisePropertyChanged("CopyOfEditedImage");
            }
        }

        public bool IsSync
        {
            get => _convertingIsSync;
            set
            {
                _convertingIsSync = value;
                RaisePropertyChanged("IsSync");
            }
        }

        private void InitLoadImage()
        {
            LoadImage = new RelayCommand(() =>
            {
                var op = new OpenFileDialog
                {
                    Title = "Select a image",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
               "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
               "Portable Network Graphic (*.png)|*.png"
                };

                if (op.ShowDialog() == true)
                {
                    ImagePath = op.FileName;
                }
            });
        }

        private void InitConvertImage()
        {
            ConvertImage = new RelayCommand(async () =>
            {
                if (_convertingIsSync)
                {
                    ImageProcessing.Grayscale(ImagePath);
                }
                else
                {
                    await ImageProcessing.GrayscaleAsync(ImagePath);
                }

                CopyOfEditedImage = GetCopyOfImage(ImagePath);
            });
        }

        private ImageSource GetCopyOfImage(string localPath)
        {
            var imageInfo = File.ReadAllBytes(localPath);
            BitmapImage image;

            using (var imageStream = new MemoryStream(imageInfo))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = imageStream;
                image.EndInit();
            }
            return image;
        }

        private void OnImageProcessingFinished(long timeInMs)
        {
            if (!System.Windows.Application.Current.CheckAccess())
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => OnImageProcessingFinished(timeInMs));
                return;
            }
            DurationOfProcessing = timeInMs.ToString();
        }
    }
}
