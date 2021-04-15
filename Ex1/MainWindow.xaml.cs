using Microsoft.Win32;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

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
            DisplayFeatureGraphControl.setMainViewModel(vm);
            CorrelateFeatureGraphControl.setMainViewModel(vm);
            this.DataContext = vm;
        }

        private void UploadCSVButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                bool completed = false;
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
            openFileDialog.Filter = "DLL Files (*.dll)|*.dll";
            if (openFileDialog.ShowDialog() == true)
            {
                var DLL = Assembly.LoadFile(openFileDialog.FileName);
                // extract interface from dll
                Type Itype = DLL.GetType("IAnomalyDetect.IAnomalyDetector");
                // find a suitable implementation of interface
                Type[] types = DLL.GetTypes();
                Type theType = null;
                foreach (Type type in types)
                {
                    if (!type.Equals(Itype) && Itype.IsAssignableFrom(type))
                    {
                        theType = type;
                        break;
                    }
                }
                try
                {
                    // use anomaly detector by invoking the interface methods
                    var learnMethod = theType.GetMethod("learn");
                    var detectMethod = theType.GetMethod("detect");
                    var instance = Activator.CreateInstance(theType);

                    learnMethod.Invoke(instance, new object[] { vm.LearnFileName });

                    Tuple<string, int>[] anomalies = (Tuple<string, int>[])detectMethod.Invoke
                                        (instance, new object[] { vm.addFeatureNamesToCSV() });
                    vm.Anomalies = anomalies;
                }
                catch (ArgumentNullException ex)
                {
                    throw new Exception("No suitable Anomaly Detector found in DLL");
                }
                // enable controls
                this.VideoControl.IsEnabled = true;
                this.FeaturesListView.IsEnabled = true;
                this.GraphReg.IsEnabled = true;
            }
        }
    }
}
