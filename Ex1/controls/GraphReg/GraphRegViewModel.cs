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
        public string VM_DisplayFeature { get { return vm.DisplayFeature; } }
        public string VM_CorrolatedFeature { get { return vm.getCorroleatedFeature(VM_DisplayFeature); } }

        public GraphRegViewModel() { }
        public override void setMainViewModel(MainViewModel vm)
        {
            base.setMainViewModel(vm);
            this.vm.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e)
               {
                   if (e.PropertyName.Equals("DisplayFeature"))
                   {
                       NotifyPropertyChanged("VM_CorrolatedFeature");
                   }
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
        }
        public Line RegLine
        {
            get { return vm.RegLine; }
        }
        public List<Point> getPoints()
        {
            if (VM_CorrolatedFeature != null)
            {
                List<float> val1 = vm.getValuesOfFeature(VM_DisplayFeature);
                List<float> val2 = vm.getValuesOfFeature(VM_CorrolatedFeature);
                List<Point> list = new List<Point>();
                for (int i = 0; i < vm.Size; i++)
                {
                    list.Add(new Point(val1[i], val2[i]));
                }
                return list;
            }
            return null;
        }
        public float DisplayFeatureMinValue
        {
            get { return vm.DisplayFeatureMinValue; }
        }
        public float CorrolateFeatureMinValue
        {
            get { return vm.CorrolateFeatureMinValue; }
        }
        public float DisplayFeatureMaxValue
        {
            get { return vm.DisplayFeatureMaxValue; }
        }
        public float CorrolateFeatureMaxValue
        {
            get { return vm.CorrolateFeatureMaxValue; }
        }
        public int Size { get { return vm.Size; } }
        public int Frequency { get { return vm.Frequency; } }
        public int LineNumber { get { return vm.LineNumber; } }
    }
}
