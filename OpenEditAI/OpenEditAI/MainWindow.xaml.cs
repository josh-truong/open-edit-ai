using FFMpegCore;
using OpenEditAI.Code;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Linq;
using OpenEditAI.Models;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OpenEditAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
        }
    }
}