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
        double size, angle;
        public Joystick()
        {
            InitializeComponent();
            JoystickInner.Margin = new Thickness(109, 56, 0, 0);
            double radius = JoystickMiddle.Margin.Left + JoystickInner.Width / 2 - JoystickMiddle.Margin.Left;
            this.vm = new JoystickViewModel(new JoystickModel(JoystickInner.Margin, radius));
            //vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e){ JoystickInner.Margin = vm.VM_NewLocation; };
            DataContext = vm;
        }

        public void setMainModel(MainController.MainModel model)
        {
            this.vm.setMainModel(model);
        }

        public double Joystick_Size
        {
            set
            {
                this.size = value;
                vm.Size = value;
            }
        }
        public double Joystick_Angle
        {
            set
            {
                this.angle = value;
                vm.Angle = value;
            }
        }
    }
}
