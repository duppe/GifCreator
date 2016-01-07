using GifCreator.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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



            //ba.Content = SymbolHelper.SegoeUiSymbol(Symbol.Accept);
            var assembly = typeof(WpfInfras.Controls.BlankWindow).Assembly;

            var iconResourceNames = from name in assembly.GetManifestResourceNames()
                                    select name;

            foreach (var item in iconResourceNames)
            {

            }
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
                try
                {
                    var source = new Uri("/GifCreator;component/Assets/ModernUI.Snowflakes.xaml", UriKind.Relative);
                    WpfInfras.Presentation.AppearanceManager.Current.ThemeSource = source;
                }
                catch (Exception ex)
                {

                    throw;
                }


                //Application.Current.Resources["Accent"] = new SolidColorBrush(color);

                //WpfInfras.Presentation.AppearanceManager.Current.AccentColor = color;
            }
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "mp4|*.mp4|flv|*.flv";
            if (dialog.ShowDialog(this) == true)
            {
                _vm.VideoSource = dialog.FileName;
                mePlayer.Play();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
