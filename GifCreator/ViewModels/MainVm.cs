using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using System;
using System.IO;
using System.Windows.Input;
using WpfInfras.Controls;
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

            ConvertCommand = new RelayCommand(Convert2Gif);
            OpenMp4Command = new RelayCommand(OpenMp4);
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

        public ICommand OpenMp4Command { get; set; }

        private void Convert2Gif()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            var inputSettings = SettingsCollection.ForInput(new StartAt(1d));
            var outputSettings = SettingsCollection.ForOutput(
                new BitRateVideo(3000),
                new DurationOutput(5d));

            var settings = SettingsCollection.ForOutput(new OverwriteOutput());

            //Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            //dialog.Title = "生成Gif";
            //dialog.DefaultExt = ".gif";
            //dialog.Filter = "gif|*.gif";
            //dialog.FileName = Path.GetFileNameWithoutExtension(VideoSource);

            var fileName = Path.Combine(Path.GetDirectoryName(VideoSource), Path.GetFileNameWithoutExtension(VideoSource) + ".gif");

            //if (dialog.ShowDialog() == true)
            {
                var factory = CommandFactory.Create();

                factory.CreateOutputCommand()
                    .WithInput<VideoStream>(VideoSource)
                    .MapTo<Gif>(fileName, settings);

                factory.Render();

                ModernDialog.ShowTips("完成！");
            }
        }

        private void OpenMp4()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "mp4|*.mp4|flv|*.flv";
            if (dialog.ShowDialog() == true)
            {
                VideoSource = dialog.FileName;
            }
        }
    }
}
