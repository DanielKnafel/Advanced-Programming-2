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
        private bool pause;
        private Thread t;
        public MainModel()
        {
            client = new Client();
            pause = false;
        }
        public string Speed
        {
            get { return $"{(double)client.getFrequency() / 10}"; }
            set
            {
                try
                {
                    client.setFrequency((int)(double.Parse(value) * 10));
                    NotifyPropertyChanged("Speed");
                }
                catch (FormatException)
                {

                }
                
            }
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
            NotifyPropertyChanged("Size");
            playVideo();
          
        }
        public void playVideo()
        {
            pause = false;
            t = new Thread(() =>
            {
                for (int i = numOfCurrentLine; i < Size; i++)
                {
                    if (pause)
                        break;
                    sendNextLine();
                    Thread.Sleep((int)(100 / double.Parse(Speed)));
                }
            });
            t.Start();
            
        }
        public void pauseVideo()
        {
            pause = true;
        }
        public void stopVideo()
        {
            pauseVideo();
            client.setNumOfCurrentLine(0);
            NotifyPropertyChanged("numOfCurrentLine");
            NotifyPropertyChanged("Time");
            NotifyPropertyChanged("CurrentLine");
        }
        public void forwardVideo(int sec)
        {
            pauseVideo();
            t.Join();
            if(numOfCurrentLine + 10 * sec > Size)
                client.setNumOfCurrentLine(Size - 1);
            else
                client.setNumOfCurrentLine(numOfCurrentLine + 10 * sec);
            NotifyPropertyChanged("numOfCurrentLine");
            NotifyPropertyChanged("Time");
            NotifyPropertyChanged("CurrentLine");
            playVideo();
        }
        public void backVideo(int sec)
        {
            pauseVideo();
            t.Join();
            if(numOfCurrentLine - 10 * sec < 0)
                client.setNumOfCurrentLine(0);
            else
                client.setNumOfCurrentLine(numOfCurrentLine - 10 * sec);
            NotifyPropertyChanged("numOfCurrentLine");
            NotifyPropertyChanged("Time");
            NotifyPropertyChanged("CurrentLine");
            playVideo();
        }
        public void prevVideo()
        {
            stopVideo();
            playVideo();
        }
        public void nextVideo()
        {
            pauseVideo();
            t.Join();
            client.setNumOfCurrentLine(Size -1);
            NotifyPropertyChanged("numOfCurrentLine");
            NotifyPropertyChanged("Time");
            NotifyPropertyChanged("CurrentLine");
        }
        public void sendNextLine()
        {
            if (client != null)
            {
                client.sendNextLine();
                NotifyPropertyChanged("CurrentLine");
                NotifyPropertyChanged("numOfCurrentLine");
                NotifyPropertyChanged("Time");
            }
        }
    }
}
