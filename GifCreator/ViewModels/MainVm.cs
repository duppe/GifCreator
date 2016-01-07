using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System;
using System.Windows.Input;
using WpfInfras.Presentation;

namespace GifCreator.ViewModels
{
    public class MainVm : NotifyPropertyChanged
    {
        private string _videoSource;


        const string outputPath = "temp";
        const string ffmpegPath = "ffmpeg.exe";
        const string ffprobePath = "ffprobe.exe";

        public MainVm()
        {
            //VideoSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "avPZqwd_460sv.mp4");

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, ffmpegPath, ffprobePath);

            ConvertCommand = new RelayCommand(Convert2Gif, () => !string.IsNullOrEmpty(VideoSource));
        }


        public string VideoSource
        {
            get { return _videoSource; }
            set
            {
                SetProperty(ref _videoSource, value, () => VideoSource);
            }
        }

        public ICommand ConvertCommand { get; set; }

        private void Convert2Gif()
        {

            var dir = AppDomain.CurrentDomain.BaseDirectory;

            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(5d));

            var settings = SettingsCollection.ForOutput(new OverwriteOutput());


            var factory = CommandFactory.Create();

            factory.CreateOutputCommand()
                .WithInput<VideoStream>(VideoSource)
                .MapTo<Gif>("d:/out.gif", settings);

            factory.Render();
        }
    }
}
