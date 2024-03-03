using FFMpegCore.Extensions.System.Drawing.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenEditAI.Code
{
    public class FFmpegUtility
    {
        public ImageSource ExtractFrames(string videoPath, TimeSpan timespan)
        {
            var bitmap = FFMpegImage.Snapshot(videoPath, new Size(200, 400), timespan);
            return ConvertBitmapToImageSource(bitmap);
        }

        public void SliceVideo(string inputPath, string outputPath, string startTime, string duration)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {inputPath} -ss {startTime} -t {duration} -c copy {outputPath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
        }

        public ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
