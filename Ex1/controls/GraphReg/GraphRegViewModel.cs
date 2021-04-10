using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Ex1.controls.GraphReg
{
    class GraphRegViewModel : ViewModel
    {
        protected string VM_displayFeature, VM_corrolatedFeature;

        public GraphRegViewModel()
        {
            VM_displayFeature = null;
            VM_corrolatedFeature = null;
        }
        public override void setDataFileReader(DataFileReader reader)
        {
            base.setDataFileReader(reader);
            this.reader.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e)
               {
                   if (e.PropertyName.Equals("LineNumber"))
                   {
                       NotifyPropertyChanged("VM_" + e.PropertyName);
                   }
                   
                   else if (e.PropertyName.Equals("displayFeature"))
                   {
                       VM_displayFeature = reader.getDisplayFeature();
                       VM_corrolatedFeature = reader.getCorroleatedFeature(VM_displayFeature);
                       NotifyPropertyChanged("VM_" + e.PropertyName);
                   }                   
                   
               };
        }
        public Polyline getRegLine()
        {
            return reader.getRegLine(VM_displayFeature, VM_corrolatedFeature);
        }
        public List<Point> getPoints()
        {
            List<float> val1 = reader.getValuesOfFeature(VM_displayFeature);
            List<float> val2 = reader.getValuesOfFeature(VM_corrolatedFeature);
            List<Point> list = new List<Point>();
            for(int i = 0; i<val1.Count(); i++)
            {
                list.Add(new Point(val1[i], val2[i]));
            }
            return list;

        }
    }
}
