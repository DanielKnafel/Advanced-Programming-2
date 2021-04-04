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
using System.ComponentModel;

namespace Ex1.controls
{
    public partial class Joystick : UserControl
    {
        private JoystickViewModel vm;
        public Joystick()
        {
            InitializeComponent();
            double radius = (JoystickMiddle.Width / 2) - (JoystickInner.Width / 2) - (Math.Sqrt(2)*JoystickInner.Width- JoystickInner.Width / 2) - JoystickInner.StrokeThickness;
            double position = (JoystickOuter.Width / 2) - JoystickInner.Width / 2;
            this.vm = new JoystickViewModel(new JoystickModel(new Point(position, position), radius));
            DataContext = vm;
        }

        public void setDataFileReader(DataFileReader reader)
        {
            this.vm.setDataFileReader(reader);
        }
    }
}
