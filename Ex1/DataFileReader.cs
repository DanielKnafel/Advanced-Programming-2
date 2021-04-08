﻿using System;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Collections.Generic;

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
        private int frequency, size;
        private volatile int lineNumber;
        // mutex for volotile resources
        private Mutex mut;
        private Dictionary<string, List<float>> featuresDictionary;

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
            definitions = new string[names.Count/2];
            for (int i = 0; i < definitions.Length; i++)
            {
                definitions[i] = names[i].InnerText;
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
                this.featuresDictionary = new Dictionary<string, List<float>>();
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
            for (int i = 0; i < definitions.Length; i++)
            {
                if (definitions[i].Equals(name))
                    return lineValues[i];
            }
            return null;
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
        }
        public string[] Definitions
        { 
            get { return this.definitions; } 
        }
        public int Size
        {
            get { return this.size; }
        }
        public Dictionary<string, List<float>> FeaturesDictionary { get { return this.featuresDictionary; } }
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
            // create list for each feature
            foreach (string name in definitions)
            {
                // make duplicates of feature names
                if (!featuresDictionary.ContainsKey(name))
                    featuresDictionary.Add(name, new List<float>());
                else
                    featuresDictionary.Add(name+"[1]", new List<float>());
            }
            // initialize lists with values
            for (int i = 0; i < data.Length; i++)
            {
                string[] lineValues = data[i].Split(',');
                for (int j = 0; j < lineValues.Length; j++)
                    featuresDictionary[definitions[j]].Add(float.Parse(lineValues[j]));
            }
        }
    }
}
