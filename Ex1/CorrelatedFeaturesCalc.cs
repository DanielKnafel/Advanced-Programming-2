using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        #region DLL Imports
        // Vector
        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern int vectorSize(IntPtr vec);

        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern IntPtr getByIndex(IntPtr vec, int index);
        // String
        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern int len(IntPtr wrap);

        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern int getCharByIndex(IntPtr wrap, int index);
        // API
        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern IntPtr CreateAPI(char[] fileName);

        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern IntPtr getCorrelationNamesVector(IntPtr api);

        [DllImport("AnomalyDetectionDLL.dll")]
        public static extern IntPtr getRegLinesVector(IntPtr api);
        #endregion

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
            IntPtr api = CreateAPI(fileName.ToCharArray());
            IntPtr namesVec = getCorrelationNamesVector(api);
            IntPtr linesVec = getRegLinesVector(api);
            int namesSize = vectorSize(namesVec);

            // get names
            for (int i = 0; i < namesSize; i++)
            {
                IntPtr namesWrap = getByIndex(namesVec, i);
                int length = len(namesWrap);
                string names = "";
                for (int j = 0; j < length; j++)
                {
                    names += (char)getCharByIndex(namesWrap, j);
                }

                IntPtr linesWrap = getByIndex(linesVec, i);
                int length2 = len(linesWrap);
                string line = "";
                for (int j = 0; j < length2; j++)
                {
                    line += (char)getCharByIndex(linesWrap, j);
                }
                string[] namesArr = names.Split(',');
                string[] lineArr = line.Split(',');

                CorrelatedFeatures c;
                c.f1 = namesArr[0];
                c.f2 = namesArr[1];
                c.regLine = new Line(float.Parse(lineArr[0]), float.Parse(lineArr[1]));
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
