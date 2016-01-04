using GifCreator.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
            Application.Current.Resources["Accent"] = new SolidColorBrush(Colour.GetSysAccentColor()) ;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "mp4|*.mp4|flv|*.flv";
            if(dialog.ShowDialog(this)  == true)
            {
                _vm.VideoSource = dialog.FileName;
                mePlayer.Play();
            }
        }
    }
}
