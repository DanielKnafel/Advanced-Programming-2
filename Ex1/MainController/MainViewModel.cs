using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ex1.controls;

namespace Ex1.MainController
{
    public class MainViewModel : ViewModel
    {
        MainModel model;

        public MainViewModel(MainModel model)
        {
            this.model = model;
        }

        public void startClient()
        {
            model.start();
        }
    }
}
