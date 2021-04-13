using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Ex1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DataFileReader reader;
        private Client client;
        private CorrelatedFeaturesCalc cfc;
        public MainViewModel()
        {
            this.reader = new DataFileReader();
            this.reader.setXMLDefinitions("playback_small.xml");
            this.reader.setCSVFile("reg_flight.csv", 10);
            this.cfc = new CorrelatedFeaturesCalc("reg_flight.csv", reader.Definitions);
            this.reader.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
            //this.client = new Client(reader);
            //this.client.connect("localhost", 5400);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public void setCSVFile(string fileName, int frequency)
        {
            this.reader.setCSVFile(fileName, frequency);
        }

        private string displayFeature;
        public string DisplayFeature
        {
            get
            {
                return this.displayFeature;
            }
            set
            {
                this.displayFeature = value;
                NotifyPropertyChanged("DisplayFeature");
                NotifyPropertyChanged("DisplayFeatureMinValue");
                NotifyPropertyChanged("CorrolateFeatureMinValue");
                NotifyPropertyChanged("DisplayFeatureMaxValue");
                NotifyPropertyChanged("CorrolateFeatureMaxValue");
                NotifyPropertyChanged("ValuesOfDisplayFeature");
                NotifyPropertyChanged("ValuesOfCorrelateFeature");
            }
        }
        public string getCorrelatedFeature(string name)
        {
            return this.cfc.getCorrelatedFeature(name);
        }
        public Line RegLine
        {
            get { return cfc.getRegLine(this.displayFeature); }
        }
        public List<float> getValuesOfFeature(string name)
        {
            return reader.getValuesOfFeature(name);
        }
        public string getValueByName(string name)
        {
            return reader.getValueByName(name);
        }
        #region Properties
        public int LineNumber
        {
            get { return reader.LineNumber; }
            set { reader.LineNumber = value; }
        }
        public string Line
        {
            get { return reader.Line; }
        }
        public double Speed
        {
            get { return reader.Speed; }
            set { reader.Speed = value; }
        }
        public int Frequency
        {
            get { return reader.Frequency; }
        }
        public List<string> Definitions
        {
            get { return reader.Definitions; }
        }
        public int Size
        {
            get { return reader.Size; }
        }
        public float DisplayFeatureMinValue
        {
            get 
            { 
                if (DisplayFeature!=null)
                    return getValuesOfFeature(DisplayFeature).Min();
                return 0;
            }
        }
        public float CorrolateFeatureMinValue
        {
            get {
                if (DisplayFeature != null && getCorrelatedFeature(DisplayFeature) != null)
                    return getValuesOfFeature(getCorrelatedFeature(DisplayFeature)).Min();
                return 0;
            }
        }
        public float DisplayFeatureMaxValue
        {
            get 
            {
                if (DisplayFeature != null)
                    return getValuesOfFeature(DisplayFeature).Max();
                return 0;
            }
        }
        public float CorrolateFeatureMaxValue
        {
            get
            {
                if (DisplayFeature != null && getCorrelatedFeature(DisplayFeature) != null)
                    return getValuesOfFeature(getCorrelatedFeature(DisplayFeature)).Max();
                return 0;
            }
        }
        public ObservableCollection<Point> ValuesOfDisplayFeature
        {
            get
            {
                if (DisplayFeature == null)
                    return new ObservableCollection<Point>();
                return ListToObservableCollection(DisplayFeature);
            }
        }
        public ObservableCollection<Point> ValuesOfCorrelateFeature
        {
            get
            {
                if (DisplayFeature == null)
                    return new ObservableCollection<Point>();
                return ListToObservableCollection(getCorrelatedFeature(DisplayFeature));
            }
        }
        #endregion
        private ObservableCollection<Point> ListToObservableCollection(string featureName)
        {
            List<float> values = getValuesOfFeature(featureName);
            ObservableCollection<Point> col = new ObservableCollection<Point>();
            for (int i = 0; i < values.Count; i++)
            {
                col.Add(new Point(i, values[i]));
            }
            return col;
        }
        public void skipForward(int seconds)
        {
            reader.skipForward(seconds);
        }
        public void skipBackwards(int seconds)
        {
            reader.skipBackwards(seconds);
        }
        public void startReading()
        {
            this.reader.startReading();
        }
        public void stopReading()
        {
            reader.stopReading();
        }
    }
}
