using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Xml;

namespace Ex1
{
    public class DataFileReader : INotifyPropertyChanged
    {
        // data from csv
        private string[] data;
        // columb names from xml
        private string[] definitions;
        private volatile bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        private string line;
        private int frequency;
        private volatile int lineNumber;

        public DataFileReader()
        {
            this.lineNumber = 0;
            line = null;
            this.frequency = 0;
            Speed = 1;
            stop = false;
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public void setXMLDefinitions(string fileName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNodeList names = xml.GetElementsByTagName("name");
            definitions = new string[names.Count];
            for (int i = 0; i < names.Count; i++)
            {
                definitions[i] = names[i].InnerText;
            }
        }
        public string getValueByName(string name)
        {
            for (int i = 0; i < definitions.Length; i++)
            {
                if (definitions[i].Equals(name))
                    return data[i];
            }
            return null;
        }
        public void startReading()
        {
            stop = false;
            new Thread(() =>
            {
                while (!stop)
                {
                    if (this.lineNumber == Size)
                        stop = true;
                    else
                    {
                        this.line = data[lineNumber];
                        NotifyPropertyChanged("Line");
                        NotifyPropertyChanged("LineNumber");
                        Thread.Sleep((int)Math.Round(1000.0 / (Frequency * Speed)));
                        lineNumber++;
                    }
                }
            }).Start();
        }
        public int LineNumber { 
            get { return this.lineNumber; }
            set
            {
                if (value < data.Length && value >= 0)
                    this.lineNumber = value;
                else
                    this.lineNumber = data.Length - 1;
            }
        }
        public string Line { 
            get { return this.line; } 
        }
        public double Speed { get; set; }
        public int Frequency
        {
            get { return this.frequency; }
        }
        public string[] Definitions { 
            get { return this.definitions; } 
        }
        public int Size
        {
            get
            {
                if (data != null)
                    return data.Length;
                return 0;
            }
        }
        public void setCSVFile(string fileName, int frequency)
        {
            try
            {
                this.data = File.ReadAllLines(fileName);
                this.frequency = frequency;
                this.lineNumber = 0;
                this.line = null;
                Speed = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void skipForward(int seconds)
        {
            int skipped = LineNumber + (seconds / Frequency) - 1;
            if (skipped < data.Length)
                this.lineNumber = skipped;
            else
                this.lineNumber = data.Length - 1;
            NotifyPropertyChanged("LineNumber");

        }
        public void skipBackwards(int seconds)
        {
            int skipped = LineNumber - (seconds / Frequency) - 1;
            if (skipped >= 0)
                this.lineNumber = skipped;
            else
                this.lineNumber = 0;
            NotifyPropertyChanged("LineNumber");
        }

        public void stopReading()
        {
            this.stop = true;
        }
    }
}
