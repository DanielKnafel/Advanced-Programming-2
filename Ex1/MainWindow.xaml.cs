using Microsoft.Win32;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

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
            Dashboard.setMainViewModel(vm);
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
                        vm.Frequency = int.Parse(UserAnswer);
                        completed = true;
                    }
                    catch (Exception suppressed) { }
                } while (!completed);
                
                this.vm.DetectFileName = openFileDialog.FileName;
                //this.vm.DetectFileName = @"C:\Users\Daniel\Source\Repos\DanielKnafel\Advanced-Programming-2\Debug\reg_flight.csv";
            }
            this.AnomalyDetectionButton.IsEnabled = true;
        }

        private void FeaturesListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vm.DisplayFeature = (string)this.FeaturesListView.SelectedValue;
        }

        private void AnomalyDetectionButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var DLL = Assembly.LoadFile(openFileDialog.FileName);
                //var DLL = Assembly.LoadFile(@"C:\Users\Daniel\Source\Repos\DanielKnafel\Advanced-Programming-2\Debug\LinearRegression.dll");
                var theType = DLL.GetType("AnomalyDetect.AnomalyDetector");
                var instance = Activator.CreateInstance(theType);
                var learnMethod = theType.GetMethod("learn");
                var detectMethod = theType.GetMethod("detect");

                learnMethod.Invoke(instance, new object[] { vm.LearnFileName });

                Tuple<string, int>[] anomalies = (Tuple<string, int>[])detectMethod.Invoke
                                    (instance, new object[] { vm.addFeatureNamesToCSV() });
                vm.Anomalies = anomalies;
                this.VideoControl.IsEnabled = true;
                this.FeaturesListView.IsEnabled = true;
            }
        }
    }
}
