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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace OpenEditAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FFmpegUtility _ffmpegUtility;
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            _ffmpegUtility = new FFmpegUtility();
            this.DataContext = _viewModel;

            //ProcessVideo();
            _ffmpegUtility.SliceVideo(@"C:\Users\joshk\Downloads\video.mp4", @"C:\Users\joshk\Downloads\video-edit.mp4", "00:00:10", "00:00:20");
        }

        public async void ProcessVideo()
        {
            string videoPath = @"C:\Users\joshk\Downloads\video.mp4";
            var videoInfo = await FFProbe.AnalyseAsync(videoPath);
            var videoDuration = videoInfo.Duration;
            for (var time = TimeSpan.Zero; time < videoDuration; time += TimeSpan.FromSeconds(1))
            {
                Trace.WriteLine(time);
                _viewModel.ImageSource = _ffmpegUtility.ExtractFrames(videoPath, time);
                await Task.Delay(1000);
            }
        }
    }
}