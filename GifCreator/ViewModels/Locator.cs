using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GifCreator.ViewModels
{
    public  class Locator
    {
        public Locator()
        {
            MainVm = new MainVm();
        }

        public MainVm MainVm { get; set; }
    }
}
