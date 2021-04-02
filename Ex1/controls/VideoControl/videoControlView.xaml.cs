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

namespace Ex1.controls
{
    /// <summary>
    /// Interaction logic for videoControlView.xaml
    /// </summary>
    public partial class videoControlView : UserControl
    {
        videoControlViewModel vm;
        private DateTime click_Started;
        public videoControlView()
        {
            InitializeComponent();
            vm = new videoControlViewModel();
            DataContext = vm;
        }
        public void setMainModel(MainController.MainModel model)
        {
            this.vm.setMainModel(model);
        }

        private void Back_Click_Down(object sender, MouseButtonEventArgs e)
        {
            click_Started = DateTime.Now;
        }
        private void Back_Click_Up(object sender, MouseButtonEventArgs e)
        {
            int seconds = (DateTime.Now.Second - click_Started.Second);
            if (seconds == 0) //short click
                vm.backVideo(1);
            else
                vm.backVideo(seconds * 10);

        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            vm.playVideo();
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            vm.pauseVideo();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            vm.stopVideo();
        }
        private void Forward_Click_Down(object sender, MouseButtonEventArgs e)
        {
            click_Started = DateTime.Now;
        }
        private void Forward_Click_Up(object sender, MouseButtonEventArgs e)
        {
            int seconds = (DateTime.Now.Second - click_Started.Second);
            if (seconds == 0) //short click
                vm.forwardVideo(1);
            else
                vm.forwardVideo(seconds * 10);
            
        }
        private void prev_Click(object sender, RoutedEventArgs e)
        {
            vm.prevVideo();
        }
        private void next_Click(object sender, RoutedEventArgs e)
        {
            vm.nextVideo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Speed = Speed.Text;
        }
    }
}
