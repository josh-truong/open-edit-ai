using FFMpegCore;
using FFMpegCore.Enums;
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

        public bool ExtractAudio(string inputPath, string outputPath)
        {
            return FFMpeg.ExtractAudio(inputPath, outputPath);
        }

        public async Task SliceVideo(string inputPath, string outputPath, int start, TimeSpan duration)
        {
            await FFMpegArguments
                .FromFileInput(inputPath, true, options => options
                    .WithVideoCodec(VideoCodec.LibX264)
                    .WithAudioCodec(AudioCodec.Aac)
                    .WithStartNumber(start)
                    .WithDuration(duration))
                .OutputToFile(outputPath, false, options => options
                    .WithVideoCodec(VideoCodec.LibX264)
                    .WithAudioCodec(AudioCodec.Aac))
                .ProcessAsynchronously();
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
