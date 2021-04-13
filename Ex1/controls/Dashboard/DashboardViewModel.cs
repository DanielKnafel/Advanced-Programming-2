using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1.controls.Dashboard
{
    public class DashboardViewModel: ViewModel
    {
        private double VM_altitude, VM_airspeed, VM_pitch, VM_roll, VM_yaw, VM_direction;

        public DashboardViewModel()
        {
            this.VM_Altitude = 0;
            this.VM_Airspeed = 0;
            this.VM_Pitch = 0;
            this.VM_Roll = 0;
            this.VM_Yaw = 0;
            this.VM_Direction = 0;
        }
        public double VM_Altitude
        {
            get { return this.VM_altitude; }
            set
            {
                if (value < 0)
                    this.VM_altitude = 0;
                else
                    this.VM_altitude = value * 36 / 100;
                NotifyPropertyChanged("VM_Altitude");
            }
        }

        public double VM_Airspeed
        {
            get { return this.VM_airspeed; }
            set
            {
                this.VM_airspeed = value;
                NotifyPropertyChanged("VM_Airspeed");
            }
        }

        public double VM_Pitch
        {
            get { return this.VM_pitch; }
            set
            {
                this.VM_pitch = value;
                NotifyPropertyChanged("VM_Pitch");
            }
        }

        public double VM_Roll
        {
            get { return this.VM_roll; }
            set
            {
                this.VM_roll = value;
                NotifyPropertyChanged("VM_Roll");
            }
        }

        public double VM_Yaw
        {
            get { return this.VM_yaw; }
            set
            {
                this.VM_yaw = value;
                NotifyPropertyChanged("VM_Yaw");
            }
        }

        public double VM_Direction
        {
            get { return this.VM_direction; }
            set
            {
                this.VM_direction = value;
                NotifyPropertyChanged("VM_Direction");
            }
        }

        public override void setMainViewModel(MainViewModel vm)
        {
            base.setMainViewModel(vm);
            this.vm.PropertyChanged += 
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("Line"))
                    {
                        try
                        {
                            this.VM_Altitude = double.Parse(vm.getValueByName("altitude-ft"));
                            this.VM_Airspeed = double.Parse(vm.getValueByName("airspeed-kt"));
                            this.VM_Pitch = double.Parse(vm.getValueByName("pitch-deg"));
                            this.VM_Roll = double.Parse(vm.getValueByName("roll-deg"));
                            this.VM_Yaw = double.Parse(vm.getValueByName("heading-deg"));
                            this.VM_Direction = double.Parse(vm.getValueByName("side-slip-deg"));
                        }
                        catch (Exception suppressed) { }
                    }
                };
        }

    }

}
