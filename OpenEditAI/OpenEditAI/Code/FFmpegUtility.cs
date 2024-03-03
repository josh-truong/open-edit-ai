using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Extensions.System.Drawing.Common;
using OpenEditAI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenEditAI.Code
{
    public class FFmpegUtility
    {
        private MainViewModel _viewModel;
        public FFmpegUtility(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public ImageSource ExtractFrames(string videoPath, TimeSpan timespan)
        {
            var bitmap = FFMpegImage.Snapshot(videoPath, new Size(200, 400), timespan);
            return ConvertBitmapToImageSource(bitmap);
        }

        public string ExtractAudio(string inputPath)
        {
            string outputMp3Path = Path.ChangeExtension(inputPath, ".mp3");
            FFMpeg.ExtractAudio(inputPath, outputMp3Path);
            return outputMp3Path;
        }

        public void SliceVideo(string inputPath, string outputPath, int start, double duration)
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            TimeSpan s = TimeSpan.FromSeconds(start);
            TimeSpan d = TimeSpan.FromSeconds(duration);
            _viewModel.Log = $"Slicing video from {s.ToString(@"hh\:mm\:ss")} to {d.ToString(@"hh\:mm\:ss")}...";

            string arguments = $"-ss {s.ToString(@"hh\:mm\:ss")} -i \"{inputPath}\" -c copy -t {d.ToString(@"hh\:mm\:ss")} \"{outputPath}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = new Process
            {
                StartInfo = processStartInfo,
            };

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

        public List<Segment> GetTimeSpanForSlice(List<int> ids, string json_file)
        {
            List<TranscribeData> speechDataList = GetJsonObject(json_file);
            List<Segment> grouped_segments = GroupSegments(ids);
            List<Segment> segments = CalculateSegments(grouped_segments, speechDataList);
            return segments;
        }

        public string MergeVideos(List<string> videos)
        { 
            foreach (string video in videos)
            {
                if (!File.Exists(video))
                    throw new FileNotFoundException($"Video file: {video} does not exists.");
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(videos[0]);
            string directoryName = Path.GetDirectoryName(videos[0]);
            string output = Path.Combine(directoryName ?? string.Empty, $"{fileNameWithoutExtension}-merged.mp4");
            FFMpeg.Join(output, videos.ToArray());
            return output;
        }

        //private List<Segment> GroupSegments(List<int> ids)
        //{
        //    var segments = new List<Segment>();
        //    for (int i = 0, start = 0; i < ids.Count; start = i)
        //    {
        //        while (++i <= ids.Count && ids[i] == ids[i - 1] + 1) ;
        //        segments.Add(new Segment { Start = ids[start], Duration = ids[i - 1] - ids[start] + 1 });
        //    }
        //    _viewModel.Log = "GROUPS: \n\t" + string.Join(", \n\t", segments);
        //    return segments;
        //}

        private List<Segment> GroupSegments(List<int> ids)
        {
            var segments = new List<Segment>();
            for (int i = 0; i < ids.Count;)
            {
                int start = i;
                while (i < ids.Count - 1 && ids[i] + 1 == ids[i + 1])
                {
                    i++;
                }
                segments.Add(new Segment { Start = ids[start], Duration = i - start + 1 });
                i++;
            }
            _viewModel.Log = "GROUPS: \n\t" + string.Join(", \n\t", segments);
            return segments;
        }

        private List<Segment> CalculateSegments(List<Segment> groups, List<TranscribeData> datas)
        {
            var segments = new List<Segment>();
            foreach (Segment group in groups)
            {
                var segment = new Segment();
                segment.Start = Get((int)group.Start, datas).Start;
                for (int i = (int)group.Start; i < (int)group.Start+(int)group.Duration; i++)
                {
                    TranscribeData data = Get(i, datas);
                    segment.Duration += data.End - data.Start;
                }
                segments.Add(segment);
            }
            _viewModel.Log = "SEGMENTS: \n\t" + string.Join(", \n\t", segments);
            return segments;
        }

        private TranscribeData Get(int i, List<TranscribeData> datas)
        {
            return datas.FirstOrDefault(data => data.Id == i);
        }

        private List<TranscribeData> GetJsonObject(string json_file)
        {
            string jsonContent = File.ReadAllText(json_file);

            // Deserialize JSON content into a list of SpeechData objects
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when deserializing properties
            };
            return JsonSerializer.Deserialize<List<TranscribeData>>(jsonContent, options);
        }
    }
}
