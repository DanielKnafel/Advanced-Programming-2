using Microsoft.Win32;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace Ex1
{
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.vm = new MainViewModel();
            Joystick.setMainViewModel(vm);
            VideoControl.setMainViewModel(vm);
            GraphReg.setMainViewModel(vm);
            //Dashboard.setMainViewModel(vm);
            this.DataContext = vm;
        }

        private void UploadCSVButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                bool completed = false;
                int frequency = 1;
                do
                {
                    string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please enter sample frequency in Hz ", "Frequency Required!", "10");
                    try
                    {
                        frequency = int.Parse(UserAnswer);
                        completed = true;
                    }
                    catch (Exception suppressed) { }
                } while (!completed);
                vm.setCSVFile(openFileDialog.FileName, frequency);
            }
            VideoControl.IsEnabled = true;
        }

        private void FeaturesListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vm.DisplayFeature = (string)this.FeaturesListView.SelectedValue;
        }
    }
}
