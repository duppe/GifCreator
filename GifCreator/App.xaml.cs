using System.Collections.Generic;
using System.Threading;
using System.Windows;
using WpfInfras.Windows;

namespace GifCreator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            SplashBootstrapper.Start(this, new MainWindow(), "Gif 生成工具", null, consume);

        }

        private IEnumerable<string> consume()
        {
            yield return "正在初始化应用"; Thread.Sleep(1500);
        }
    }
}
