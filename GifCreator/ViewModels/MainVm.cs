using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Filters;
using Hudl.FFmpeg.Filters.BaseTypes;
using Hudl.FFmpeg.Metadata;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using WpfInfras.ViewModels;

namespace GifCreator.ViewModels
{
    public class MainVm:BaseVm
    {
        private string _videoSource;
        private Bitmap _outputBitmap;


        const string outputPath = "temp";
        const string ffmpegPath = "ffmpeg.exe";
        const string ffprobePath = "ffprobe.exe";

        public MainVm()
        {
            VideoSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "avPZqwd_460sv.mp4");

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(outputPath, ffmpegPath, ffprobePath);

            ConvertCommand = new RelayCommand(Convert2Gif, () => !string.IsNullOrEmpty(VideoSource));
        }

     
        public string VideoSource
        {
            get { return _videoSource; }
            set
            {
                SetProperty(ref _videoSource, value,()=> VideoSource);
            }
        }

        public ICommand  ConvertCommand { get; set; }

        private void Convert2Gif()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(5d));

            var settings = SettingsCollection.ForOutput(new OverwriteOutput());
            

            var factory = CommandFactory.Create();

            factory
                .CreateOutputCommand()
                .WithInput<VideoStream>(VideoSource)
                 .MapTo<Gif>("d:/out.gif", settings);

            factory.Render();
        }


   
        public Bitmap OutputBitmap
        {
            get { return _outputBitmap; }
            set { SetProperty(ref _outputBitmap, value,()=> OutputBitmap); }
        }
    }
}
