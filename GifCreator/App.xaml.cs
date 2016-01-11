using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace GifCreator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception);
            e.SetObserved();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //e.ExceptionObject;

            Debug.WriteLine(e.ExceptionObject);



#if !DEBUG
            //程序即将退出。
            Environment.Exit(-1);
#endif
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (!e.Handled)
            {
                //在App_DispatcherUnhandledException未处理的异常
                WpfInfras.Windows.ExceptionViewer exViewer = new WpfInfras.Windows.ExceptionViewer(e.Exception);

                var handled = exViewer.ShowDialog() == true;

                e.Handled = handled;
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            WpfInfras.Windows.ExceptionViewer exViewer = new WpfInfras.Windows.ExceptionViewer(e.Exception);

            var handled = exViewer.ShowDialog() == true;

            e.Handled = handled;
        }
    }
}
