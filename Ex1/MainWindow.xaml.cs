using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using Ex1.MainController;

namespace Ex1
{
    public partial class MainWindow : Window
    {
        MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            MainModel mainModel = new MainModel();
            vm = new MainViewModel(mainModel);
            Joystick.setMainModel(mainModel);
            VideoControl.setMainModel(mainModel);
            mainModel.start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.startClient();
        }

    }
}
