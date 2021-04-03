using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ex1.controls
{
    public class JoystickModel : INotifyPropertyChanged
    {
        private double radios;
        private Thickness defaultLocation;
        private Thickness newLocation;

        public JoystickModel(Thickness defaultLocation, double radius)
        {
            this.defaultLocation = defaultLocation;
            this.newLocation = defaultLocation;
            this.radios = radius;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public Thickness NewLocation
        {
            get { return this.newLocation; }
            set
            {
                this.newLocation = value;
                NotifyPropertyChanged("NewLocation");
            }
        }
        public Thickness DefaultLocation
        {
            get; set;
        }

        // moves the inner joystick to the given position. 0 <= size <= 1, 0 <= angle < 360
        public void moveJoystick(double size, double angle)
        {
            angle *= Math.PI / 180;
            size *= this.radios;
            double newCenterX = size * Math.Sin(angle);
            double newCenterY = size * Math.Cos(angle);
            this.NewLocation = new Thickness(this.defaultLocation.Left + newCenterX, this.defaultLocation.Top - newCenterY, this.defaultLocation.Right, this.defaultLocation.Bottom);
        }
    }
}

