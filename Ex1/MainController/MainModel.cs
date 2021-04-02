using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex1.MainController
{
    public class MainModel : INotifyPropertyChanged
    {
        private Client client;
        private FlightData flightData;

        public MainModel() { }

        public string CurrentLine
        {
            get { return client.getCurrentLine(); }
        }

        public FlightData FlightData

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
            client.start();
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
