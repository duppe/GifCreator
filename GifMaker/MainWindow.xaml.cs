using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

using Hudl.FFmpeg;
using Hudl.FFmpeg.Command;
using Hudl.FFmpeg.Resources;
using Hudl.FFmpeg.Resources.BaseTypes;
using Hudl.FFmpeg.Settings;
using Hudl.FFmpeg.Settings.BaseTypes;
using Hudl.FFmpeg.Sugar;
using Hudl.FFmpeg.Attributes;
using Hudl.FFmpeg.Logging;
using Hudl.FFmpeg.Resources.Interfaces;
using Hudl.FFmpeg.Settings.Attributes;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;


namespace GifMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private const string TempPath = "temp";
        private const string OutputPath = "output";

        private readonly Tuple<int, int>[] _sizeDict =
        {
            new Tuple<int, int>(-1, -1)
            , new Tuple<int, int>(600, -1)
            , new Tuple<int, int>(500, -1)
            , new Tuple<int, int>(480, -1)
            , new Tuple<int, int>(320, -1)
            , new Tuple<int, int>(-1, 480)
            , new Tuple<int, int>(-1, 320)
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            FFmePlayer.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "test.mp4");
//            FFmePlayer.Source = new Uri("https://zippy.gfycat.com/HairyIdenticalCougar.webm"); 
            FFmePlayer.Play();


            string ffmpegPath = Unosquare.FFmpegMediaElement.MediaElement.FFmpegPaths.FFmpeg;
            string ffprobePath = Unosquare.FFmpegMediaElement.MediaElement.FFmpegPaths.FFprobe;

            ResourceManagement.CommandConfiguration = CommandConfiguration.Create(TempPath, ffmpegPath, ffprobePath);

            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            FFmePlayer.Close();
        }

        private void MoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsFlyout.IsOpen = !SettingsFlyout.IsOpen;
        }

        private void SettingsFlyout_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }


        private void FFmePlayer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void FFmePlayer_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        private void FFmePlayer_DragOver(object sender, DragEventArgs e)
        {

        }

        private void FFmePlayer_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if ((null == droppedFiles) || (!droppedFiles.Any())) { return; }

            var file = droppedFiles.First();

            FFmePlayer.Source = new Uri(file); 
        }

        private async void MakeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var source = FFmePlayer.Source;
            var fileName = Path.GetFileName(source.AbsoluteUri);
            var tempPath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "temp");
            var targetFile = source.AbsolutePath;
            if (!source.IsFile)
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        targetFile = Path.Combine(tempPath, fileName+".mp4");
                        webClient.DownloadFile(source.AbsoluteUri, targetFile);
                    }
                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("tips", "can not download from url:" + source.AbsoluteUri);
                    return;
                }
                
            }

            FileInfo fileInfo = new FileInfo(targetFile);
            if (fileInfo.Length > 10 * 1024 * 1024)
            {
                await this.ShowMessageAsync("too large", "the video file is large more than 10mb.");
            }

            var gifFileName  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory , OutputPath, Path.GetFileNameWithoutExtension(fileName) + ".gif");

            var index = SizeComboBox.SelectedIndex ;
            var size = _sizeDict[index];

            double frameRate;

            if (!double.TryParse(FrameRateTextBox.Text, out frameRate))
            {
                frameRate = 10;
            }
            var outputSettings = SettingsCollection.ForOutput(
                new CustomFilter(string.Format("\"scale={0}:{1}\"", size.Item1, size.Item2))
                , new FrameRate(frameRate)
                , new OverwriteOutput());

            try
            {
                var factory = CommandFactory.Create();
                factory.CreateOutputCommand()
                    .WithInput<VideoStream>(targetFile)
                    .MapTo<Gif>(gifFileName, outputSettings);


                factory.Render();

                Clipboard.SetText(gifFileName);

                await this.ShowMessageAsync("tips", "make success. gif path was copyed to clipboard");
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void FFmePlayer_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                if (this.FFmePlayer.IsPlaying)
                    this.FFmePlayer.Pause();
                else
                    this.FFmePlayer.Play();

                return;
            }

            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.FFmePlayer.Stop();
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FFmePlayer.Source = new Uri(SourceText.Text);
            }
            catch
            {

            }
        }
    }

    [ContainsStream(Type = typeof (VideoStream))]
    [ContainsStream(Type = typeof (DataStream))]
    public class Gif : BaseContainer
    {
        private const string FileFormat = ".gif";

        public Gif()
            : base(FileFormat)
        {
        }

        protected override Hudl.FFmpeg.Resources.Interfaces.IContainer Clone()
        {
            return new Gif();
        }
    }

    [ForStream(Type = typeof (VideoStream))]
    [Setting(Name = "vf")]
    public class CustomFilter : BaseBitStreamFilter
    {
        public CustomFilter(string setting)
            : base(setting)
        {
        }
    }
}
