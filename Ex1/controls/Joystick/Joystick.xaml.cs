using System;
using System.Windows;
using System.Windows.Controls;

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
