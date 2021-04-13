using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnomalyDetectionDLL;

namespace Ex1
{
    public struct CorrelatedFeatures
    {
        public string f1, f2;
        public Line regLine;
    }
    public class CorrelatedFeaturesCalc
    {
        private List<CorrelatedFeatures> cf;

        public CorrelatedFeaturesCalc(string fileName, List<string> definitions)
        {
            this.cf = new List<CorrelatedFeatures>();
            setCorrelatedFeaturesMap(addFeatureNamesToCSV(fileName, definitions));
        }


        private string addFeatureNamesToCSV(string fileName, List<string> definitions)
        {
            string names = "";
            foreach (string s in definitions)
                names += s + ',';
            // remove last ,
            names = names.Substring(0, names.Length - 1);
            // Create a new file     
            using (StreamWriter sw = File.CreateText("1"+fileName))
            {
                sw.WriteLine(names);
                // copy content of original CSV
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                    sw.WriteLine(line);
            }
            return "1" + fileName;
        }
        public void setCorrelatedFeaturesMap(string fileName)
        {
            API api = new API(fileName);
            Tuple<string, string>[] namesArr = api.getCorrelationNamesVector();
            Tuple<float, float>[] linesArr = api.getRegLinesVector();

            for (int i = 0; i < namesArr.Length; i++)
            {
                CorrelatedFeatures c;
                c.f1 = namesArr[i].Item1;
                c.f2 = namesArr[i].Item2;
                c.regLine = new Line(linesArr[i].Item1, linesArr[i].Item2);
                cf.Add(c);
            }
        }  

        public string getCorrelatedFeature(string name)
        {
            foreach (CorrelatedFeatures c in this.cf)
            {
                if (c.f1.Equals(name))
                    return c.f2;
                if(c.f2.Equals(name))
                    return c.f1;
            }
            return null;
        }

        public Line getRegLine(string name)
        {
            for (int i = 0; i < cf.Count; i++)
            {
                if (cf[i].f1.Equals(name) || cf[i].f2.Equals(name))
                    return cf[i].regLine;
            }
            return new Line();
        }
    }
}
