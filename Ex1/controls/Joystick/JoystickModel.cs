using System.ComponentModel;
using System.Windows;

namespace Ex1.controls
{
    public class JoystickModel : INotifyPropertyChanged
    {
        private double radius;
        private Point defaultLocation;
        private Point newLocation;

        public JoystickModel(Point defaultLocation, double radius)
        {
            this.defaultLocation = defaultLocation;
            this.newLocation = defaultLocation;
            this.radius = radius;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public Point NewLocation
        {
            get { return this.newLocation; }
            set
            {
                this.newLocation = value;
                NotifyPropertyChanged("NewLocation");
            }
        }
        public Point DefaultLocation
        {
            get; set;
        }

        // moves the inner joystick to the given position.
        public void moveJoystick(double aileron, double elevator)
        {
            this.NewLocation = new Point(this.defaultLocation.X - this.radius * aileron, this.defaultLocation.Y + this.radius * elevator);
        }
    }
}

