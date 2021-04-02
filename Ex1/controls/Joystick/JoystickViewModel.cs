using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Ex1.controls
{
    public class JoystickViewModel : ViewModel
    {
        private JoystickModel model;
        private DataFileReader reader;
        private double size, angle;

        public JoystickViewModel(JoystickModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public void setDataFileReader(DataFileReader reader)
        {
            this.reader = reader;
            this.reader.PropertyChanged +=
                    delegate (Object sender, PropertyChangedEventArgs e)
                    {
                        if (e.PropertyName.Equals("Line"))
                            reader.Line;
                            //model.moveJoystick(this.size, this.angle);
                    };
        }

        public Thickness VM_NewLocation
        {
            get { return model.NewLocation; }
        }
        public Thickness VM_DefaultLocation
        {
            set { model.DefaultLocation = value; }
        }
        public double Size
        {
            set
            {
                this.size = value;
                model.moveJoystick(this.size, this.angle);
            }
        }
        public double Angle
        {
            set
            {
                this.angle = value;
                model.moveJoystick(this.size, this.angle);
            }
        }
    }
}
