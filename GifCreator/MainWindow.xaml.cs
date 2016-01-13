using GifCreator.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using WpfInfras.Windows;

namespace GifCreator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private MainVm _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = (MainVm)DataContext;
            mePlayer.MediaEnded += MePlayer_MediaEnded;
        }

        private void MePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mePlayer.Position = new TimeSpan(0, 0, 0);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();

            //设置主题
            var source = new Uri("/GifCreator;component/Assets/ModernUI.Snowflakes.xaml", UriKind.Relative);
            WpfInfras.Presentation.AppearanceManager.Current.ThemeSource = source;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SplashWindow ss = new SplashWindow();
            ss.Show();

            Thread.Sleep(3 * 1000);
            ss.Close();
        }

        private void mePlayer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void mePlayer_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        private void mePlayer_DragOver(object sender, DragEventArgs e)
        {

        }

        private void mePlayer_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if ((null == droppedFiles) || (!droppedFiles.Any())) { return; }

            var file = droppedFiles.First();

            _vm.VideoSource = file;

        }
    }
}
