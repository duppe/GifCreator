using WpfInfras.TinyIoC;

namespace GifCreator.ViewModels
{
    public class Locator
    {
        private TinyIoCContainer _container = TinyIoCContainer.Current;

        public Locator()
        {
            MainVm = Resolve<MainVm>();
        }

        public MainVm MainVm { get; set; }


        private T Resolve<T>() where T : class
        {
            var instance = _container.Resolve<T>();
            _container.Register<T>(instance);
            return instance;
        }
    }
}
