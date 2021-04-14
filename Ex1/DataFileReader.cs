using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace Ex1
{
    public class DataFileReader : INotifyPropertyChanged
    {
        // data from csv
        private string[] data;
        // columb names from xml
        private volatile bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        private string line;
        private int frequency, size;
        private volatile int lineNumber;
        // mutex for volotile resources
        private Mutex mut;
        private Dictionary<string, List<float>> featuresDictionary;
        private Dictionary<string, int> featuresColumbs;


        public DataFileReader()
        {
            this.lineNumber = 0;
            line = null;
            this.frequency = 1;
            Speed = 1;
            stop = false;
            mut = new Mutex();
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
            this.featuresColumbs = new Dictionary<string, int>();
            this.featuresDictionary = new Dictionary<string, List<float>>();
            for (int i = 0; i < names.Count/2; i++)
            {
                string name = names[i].InnerText;
                // make duplicates of feature names
                if (!featuresColumbs.ContainsKey(name))
                {
                    featuresColumbs.Add(name, i);
                    featuresDictionary.Add(name, new List<float>());
                }
                else
                {
                    featuresColumbs.Add(name + "[1]", i);
                    featuresDictionary.Add(name + "[1]", new List<float>());
                }
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
                this.size = this.data.Length;
                NotifyPropertyChanged("Size");
                Speed = 1;
                setFeaturesDictionary();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public string getValueByName(string name)
        {
            string[] lineValues = this.line.Split(',');
            return lineValues[featuresColumbs[name]];
        }
        public void startReading()
        {
            mut.WaitOne();
            stop = false;
            mut.ReleaseMutex();
            new Thread(() =>
            {
                while (!stop)
                {
                    if (this.lineNumber == Size)
                    {
                        mut.WaitOne();
                        stop = true;
                        mut.ReleaseMutex();
                    }
                    else
                    {
                        this.line = data[lineNumber];
                        NotifyPropertyChanged("Line");
                        Thread.Sleep((int)Math.Round(1000.0 / (Frequency * Speed)));
                        mut.WaitOne();
                        LineNumber++;
                        mut.ReleaseMutex();
                    }
                }
            }).Start();
        }
        #region Properties
        public int LineNumber
        {
            get { return this.lineNumber; }
            set
            {
                mut.WaitOne();
                if (value < data.Length && value >= 0)
                    this.lineNumber = value;
                else
                    this.lineNumber = data.Length - 1;
                mut.ReleaseMutex();
                NotifyPropertyChanged("LineNumber");
            }
        }
        public string Line
        { 
            get { return this.line; } 
        }
        public double Speed
        {
            get; set;
        }
        public int Frequency
        {
            get { return this.frequency; }
            set { this.frequency = value; }
        }
        public List<string> Definitions
        { 
            get { return new List<string>(featuresColumbs.Keys); } 
        }
        public int Size
        {
            get { return this.size; }
        }
        #endregion
        public void skipForward(int seconds)
        {
            int skipped = LineNumber + (seconds * Frequency) - 1;
            mut.WaitOne();
            if (skipped < data.Length)
                this.LineNumber = skipped;
            else
                this.LineNumber = data.Length - 1;
            mut.ReleaseMutex();
        }
        public void skipBackwards(int seconds)
        {
            int skipped = LineNumber - (seconds * Frequency) - 1;
            mut.WaitOne();
            if (skipped >= 0)
                this.LineNumber = skipped;
            else
                this.LineNumber = 0;
            mut.ReleaseMutex();
        }
        public void stopReading()
        {
            mut.WaitOne();
            this.stop = true;
            mut.ReleaseMutex();
        }
        // returns a mapping between feature name and its values vector
        private void setFeaturesDictionary()
        {
            List<string> names = new List<string>(featuresColumbs.Keys);
            // initialize lists with values
            foreach (string name in names)
            {
                featuresDictionary[name] = new List<float>();
            }
            for (int i = 0; i < data.Length; i++)
            {
                string[] lineValues = data[i].Split(',');
                for (int j = 0; j < lineValues.Length; j++)
                    featuresDictionary[names[j]].Add(float.Parse(lineValues[j]));
            }
        }
        public List<float> getValuesOfFeature(string name)
        {
            if (name != null && this.featuresDictionary.ContainsKey(name))
               return this.featuresDictionary[name];
            return new List<float>();
        }
        public string addFeatureNamesToCSV()
        {
            string names = "";
            foreach (string s in this.Definitions)
                names += s + ',';
            // remove last ,
            names = names.Substring(0, names.Length - 1);
            // Create a new file     
            using (StreamWriter sw = File.CreateText("temp.csv"))
            {
                sw.WriteLine(names);
                // copy content of original CSV
                foreach (string line in this.data)
                    sw.WriteLine(line);
            }
            return "temp.csv";
        }
    }
}
