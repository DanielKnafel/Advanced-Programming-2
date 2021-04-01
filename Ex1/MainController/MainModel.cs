using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1.MainController
{
    public class MainModel : INotifyPropertyChanged
    {
        private Client client;

        public MainModel()
        {
            client = new Client();
        }
        public int Size
        {
            get { return client.dataSize(); }
        }
        public string CurrentLine
        {
            get { return client.getCurrentLine(); }
        }
        public int numOfCurrentLine
        {
            get { return client.getNumOfCurrentLine(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void start()
        {
            client = new Client();
            client.connect("localhost", 5400);
            client.setData("reg_flight.csv");
        }
        public void sendNextLine()
        {
            if (client != null)
            {
                client.sendNextLine();
                NotifyPropertyChanged("CurrentLine");
            }
        }
    }
}
