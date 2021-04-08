using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ex1
{
    public class PearsonCalculator
    {
        public class Line
        {
            public float A { get; set; }
            public float B { get; set; }
            public Line(float a, float b)
            {
                this.A = a;
                this.B = b;
            }
        }
        class CorrelatedFeatures
        {
            // names of the correlated features
            public string Feature1 { get; set; }
            public string Feature2 { get; set; }
            public float Corrlation { get; set; }
            public Line Reg_line { get; set; }
            //float threshold;
            //float cx, cy;
            public CorrelatedFeatures(string feature1, string feature2, float corrlation, Line reg_line)
            {
                this.Feature1 = feature1;
                this.Feature2 = feature2;
                this.Corrlation = corrlation;
                this.Reg_line = reg_line;
            }
        }
        
        private List<CorrelatedFeatures> cf;

        public PearsonCalculator(DataFileReader reader)
        {
            this.cf = new List<CorrelatedFeatures>();
            setCorrelativeFeatures(reader.getFeaturesDictionary());
        }
        private void setCorrelativeFeatures(Dictionary<string, List<float>> featuresDictionary)
        {
            string f1 = null, f2 = null;
            float max = 0;
            ICollection<string> keys = featuresDictionary.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                 f1 = keys.ElementAt(i);
                max = 0;
                int jmax = 0;
                for (int j = i + 1; j < keys.Count; j++)
                {
                    float p = Math.Abs(pearson(featuresDictionary.ElementAt(i).Value, featuresDictionary.ElementAt(j).Value));
                    if (p > max)
                    {
                        max = p;
                        jmax = j;
                    }
                }
                f2 = featuresDictionary.ElementAt(jmax).Key;
            }
            this.cf.Add(new CorrelatedFeatures(f1, f2, max, linear_reg(featuresDictionary[f1], featuresDictionary[f2])));
        }
        public string getMostCorrelative(string featureName)
        {
            if (this.cf == null)
                return null;
            foreach (CorrelatedFeatures c in this.cf)
            {
                if (c.Feature1.Equals(featureName))
                    return c.Feature2;
                else if (c.Feature2.Equals(featureName))
                    return c.Feature1;
            }
            return null;
        }
        private float avg(List<float> x)
        {
            float sum = 0;
            foreach (float f in x)
                sum += f;
            return sum / x.Count;
        }
        // returns the variance of X and Y
        private float var(List<float> x)
        {
            int size = x.Count;
            float av = avg(x);
            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }
        // returns the covariance of X and Y
        private float cov(List<float> x, List<float> y)
        {
            float sum = 0;
            int size = x.Count;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - avg(x) * avg(y);
        }
        // returns the Pearson correlation coefficient of X and Y
        private float pearson(List<float> x, List<float> y)
        {
            return cov(x, y) / (float)(Math.Sqrt(var(x) * var(y)));
        }
        // performs a linear regression and returns the line equation
        Line linear_reg(List<float> x, List<float> y)
        {
            float a = cov(x, y) / var(x);
            float b = avg(y) - a * (avg(x));
            return new Line(a, b);
        }
    }
}
