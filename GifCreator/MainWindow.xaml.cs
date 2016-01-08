using GifCreator.ViewModels;
using System;
using System.Windows;
using WpfInfras.PInvoke;

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

        /// <summary>
        /// The resource key for the accent color.
        /// </summary>
        public const string KeyAccentColor = "AccentColor";
        /// <summary>
        /// The resource key for the accent brush.
        /// </summary>
        public const string KeyAccent = "Accent";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();


            var color = Colour.GetSysAccentColor();

            if (color.R + color.G + color.B > 20)
            {
                var source = new Uri("/GifCreator;component/Assets/ModernUI.Snowflakes.xaml", UriKind.Relative);
                WpfInfras.Presentation.AppearanceManager.Current.ThemeSource = source;
            }
        }


    }
}
