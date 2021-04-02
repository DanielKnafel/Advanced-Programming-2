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

namespace Ex1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataFileReader reader = new DataFileReader();
            Client client = new Client(reader);

            // add option for user specified file selection

            int frequency = 10;
            reader.setFile("", frequency);
            reader.setXMLDefinitions("playback_small.xml");
            Joystick.setDataFileReader(reader);
            //VideoControl.setDataFileReader(reader);
        }
    }
}
