using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ex1
{
    public class CorrelatedFeaturesCalc
    {
        private List<KeyValuePair<string, string>> correlatedFeatures;
        public CorrelatedFeaturesCalc(string fileName)
        {
            this.correlatedFeatures = new List<KeyValuePair<string, string>>();
            setCorrelatedFeaturesMap(fileName);
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
        public static extern IntPtr getVector(IntPtr api);
        #endregion
        public void setCorrelatedFeaturesMap(string fileName)
        {
            IntPtr api = CreateAPI(fileName.ToCharArray());
            IntPtr vec = getVector(api);
            int size = vectorSize(vec);

            for (int i = 0; i < size; i++)
            {
                IntPtr strWrap = getByIndex(vec, i);
                int length = len(strWrap);
                string str = "";
                for (int j = 0; j < length; j++)
                {
                    str += getCharByIndex(strWrap, j);
                }
                string[] arr = str.Split(',');
                correlatedFeatures.Add(new KeyValuePair<string, string>(arr[0], arr[1]));
            }
        }
        
        public string getCorrelatedFeature(string name)
        {
            foreach (KeyValuePair<string, string> pair in this. correlatedFeatures)
            {
                if (pair.Key.Equals(name))
                    return pair.Value;
                if(pair.Value.Equals(name))
                    return pair.Key;
            }
            return null;
        }
    }
}
