using Microsoft.Win32;
using System;
using System.Windows;
using System.Runtime.InteropServices;

namespace Ex1
{
    public partial class MainWindow : Window
    {
        private DataFileReader reader;
        public MainWindow()
        {
            InitializeComponent();
            reader = new DataFileReader();
            //reader.setCSVFile("reg_flight.csv", frequency);
            reader.setXMLDefinitions("playback_small.xml");
            //Client client = new Client(reader);
            //client.connect("localhost", 5400);
            Joystick.setDataFileReader(reader);
            VideoControl.setDataFileReader(reader);
            VideoControl.IsEnabled = false;
        }

        private void UploadXMLButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                reader.setXMLDefinitions(openFileDialog.FileName);
            UploadCSVButton.IsEnabled = true;
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
                reader.setCSVFile(openFileDialog.FileName, frequency);
            }
            VideoControl.IsEnabled = true;
        }
    }
}
