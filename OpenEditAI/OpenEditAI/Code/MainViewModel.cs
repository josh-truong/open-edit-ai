using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenEditAI.Code 
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private ImageSource? _imageSource;
        public ImageSource? ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ChangeImageSource();
        }

        [RelayCommand]
        private void ChangeImageSource()
        {
            string newImagePath = @"C:\Users\joshk\Downloads\1707829626461.jpg";
            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(newImagePath, UriKind.Absolute);
            bitmapImage.EndInit();
            ImageSource = bitmapImage;
            Trace.WriteLine(ImageSource);
        }

        int CalculateDaysBetweenDates(DateTime date) { 
        }

        #pragma warning disable CS8612
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
