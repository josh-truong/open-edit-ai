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

        #pragma warning disable CS8612
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
