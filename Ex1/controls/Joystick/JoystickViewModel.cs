using System;
using System.ComponentModel;

namespace Ex1.controls
{
    public class JoystickViewModel : ViewModel
    {
        private JoystickModel model;
        private double aileron, elevator, rudder, throttle;
        private int VM_newLocation_X, VM_newLocation_Y;

        public JoystickViewModel(JoystickModel model)
        {
            this.model = model;
            this.VM_NewLocation_X = (int)model.NewLocation.X;
            this.VM_NewLocation_Y = (int)model.NewLocation.Y;

            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("NewLocation"))
                    {
                        this.VM_NewLocation_X = (int)model.NewLocation.X;
                        this.VM_NewLocation_Y = (int)model.NewLocation.Y;
                    }
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public override void setDataFileReader(DataFileReader reader)
        {
            this.reader = reader;
            this.reader.PropertyChanged +=
                    delegate (Object sender, PropertyChangedEventArgs e)
                    {
                        if (e.PropertyName.Equals("Line"))
                        {
                            try
                            {
                                this.Throttle = double.Parse(reader.getValueByName("throttle"));
                                this.Rudder = double.Parse(reader.getValueByName("rudder"));
                                this.Aileron = double.Parse(reader.getValueByName("aileron"));
                                this.Elevator = double.Parse(reader.getValueByName("elevator"));
                            }
                            catch (Exception suppressed) { }
                        }
                    };
        }

        public int VM_NewLocation_X
        {
            get { return this.VM_newLocation_X; }
            set { 
                this.VM_newLocation_X = value;
                NotifyPropertyChanged("VM_NewLocation_X");
            }
        }
        public int VM_NewLocation_Y
        {
            get { return this.VM_newLocation_Y; }
            set 
            { 
                this.VM_newLocation_Y = value;
                NotifyPropertyChanged("VM_NewLocation_Y");
            }
        }
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                this.aileron = value;
                model.moveJoystick(this.aileron, this.elevator);
            }
        }
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                this.elevator = value;
                model.moveJoystick(this.aileron, this.elevator);
            }
        }
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                this.rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                this.throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }
    }
}
