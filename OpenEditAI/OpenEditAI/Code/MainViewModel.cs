using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using OpenEditAI.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace OpenEditAI.Code 
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private FFmpegUtility _ffmpegUtility;
        private OpenAIUtility _openAIUtility;

        public MainViewModel()
        {
            _ffmpegUtility = new FFmpegUtility(this);
            _openAIUtility = new OpenAIUtility(this);
        }

        private string _prompt = "As a tech-focused YouTube creator, I need your help selecting the most engaging parts from a conversation transcript for a video segment. Delve into the transcript, identify key moments.";
        public string Prompt
        {
            get { return _prompt; }
            set
            {
                _prompt = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged();
                });
            }
        }

        private string _selected = String.Empty;
        public string Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged();
                });
            }
        }

        private string _log = String.Empty;
        public string Log
        {
            get { return _log; }
            set 
            {
                Trace.WriteLine(value);
                _log += value + Environment.NewLine;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged();
                });
            }
        }

        private ImageSource? _imageSource;
        public ImageSource? ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnPropertyChanged();
                });
            }
        }

        [RelayCommand]
        public async void Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP4 files (*.mp4)|*.mp4";
            if (openFileDialog.ShowDialog() == true)
            {
                Selected = openFileDialog.FileName;
            }
        }

        [RelayCommand]
        public async Task ProcessVideo()
        {
            string source = @"C:\Users\joshk\Downloads\LoW.mp4";
            Log = $"Source: {source}";
            string audio = await Task.Run(() => ExtractAudioStream(source));
            string transcript = await Task.Run(() => TranscribeAudio(audio));
            List<int> srt_ids = await Task.Run(() => ExtractSRT(transcript));
            List<string> videos = await Task.Run(() => SliceVideo(source, srt_ids, transcript));
            string final = await Task.Run(() => MergeVideo(videos));

            var psi = new ProcessStartInfo()
            {
                FileName = final,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        public string ExtractAudioStream(string source)
        {
            if (Path.GetExtension(source) != ".mp4") Application.Current.Shutdown();

            Log = "Extracting Audio...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string audio = _ffmpegUtility.ExtractAudio(source);
            Log = $"Audio file: {audio}";

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            Log = $"Finished in {timeTaken.ToString()}...";
            return audio;
        }

        public string TranscribeAudio(string source)
        {
            if (Path.GetExtension(source) != ".mp3") Application.Current.Shutdown();

            Log = "Transcribing Audio File...";
            Log = $"Source File: {source}";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string transcript = _openAIUtility.GetTranscription(source);
            Log = $"Transcript file: {transcript}";

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            Log = $"Finished in {timeTaken}...";
            return transcript;
        }

        public List<int> ExtractSRT(string source)
        {
            if (Path.GetExtension(source) != ".json") System.Windows.Application.Current.Shutdown();

            Log = "Extracting SRT File...";
            Log = $"Source File: {source}";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<int> srt_ids = _openAIUtility.GetExtractedSubtitles(source, Prompt);
            Log = $"SRT IDS: {string.Join(", ", srt_ids)}";

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            Log = $"Finished in {timeTaken}...";
            return srt_ids;
        }

        public List<string> SliceVideo(string source, List<int> ids, string json_file)
        {
            Log = "Slicing Video...";
            Log = $"Source File: {source}";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<string> sliced_videos = new List<string>();
            List<Segment> segments = _ffmpegUtility.GetTimeSpanForSlice(ids, json_file);
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                string input = source;
                string outputFilename = Path.GetFileNameWithoutExtension(input) + "-slice" + i + Path.GetExtension(input);
                string outputPath = Path.Combine(Path.GetDirectoryName(input), outputFilename);
                _ffmpegUtility.SliceVideo(input, outputPath, (int)segment.Start, segment.Duration);
                sliced_videos.Add(outputPath);
            }

            Log = "VIDEOS: \n\t" + string.Join(", \n\t", sliced_videos);

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            Log = $"Finished in {timeTaken}...";

            return sliced_videos;
        }

        public string MergeVideo(List<string> sources)
        {
            Log = "Merging Videos...";
            Log = $"Source File: {sources}";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string merged = _ffmpegUtility.MergeVideos(sources);
            Log = $"Final file: {merged}";

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            Log = $"Finished in {timeTaken}...";

            return merged;
        }

        #pragma warning disable CS8612
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
