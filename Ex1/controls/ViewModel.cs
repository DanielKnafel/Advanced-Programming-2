using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1.controls
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected DataFileReader reader;
        public virtual void setDataFileReader(DataFileReader reader)
        {
            this.reader = reader;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
