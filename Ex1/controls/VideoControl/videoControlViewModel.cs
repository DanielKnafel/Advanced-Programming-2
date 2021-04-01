using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace Ex1.controls
{
    class videoControlViewModel : ViewModel
    {
       // private TimeSpan time;
        private DateTime dt;
        private int seconds; //double??
        public double VM_Size
        {
            get { return mainModel.Size / 10; }
        }
        public double VM_Sec
        {
            get
            {
                return mainModel.numOfCurrentLine / 10;
            }
            set
            {

            }
        }
        public string VM_Time
        {
            get
            {
                seconds = mainModel.numOfCurrentLine / 10;
                int m = seconds / 60;
                int s = seconds - m * 60;
                string str;
                if (m < 10 && s < 10)
                    str = $"0{m.ToString()} :  0{(s).ToString()}";
                else if(m < 10)
                    str = $"0{m.ToString()} :  {(s).ToString()}";
                else if(s < 10)
                    str = $"{m.ToString()} :  0{(s).ToString()}";
                else
                    str = $"{m.ToString()} :  {(s).ToString()}";
                return str;
            }
            
        }
        private MainController.MainModel mainModel;
        public videoControlViewModel()
        {
            seconds = 200;
            dt = new DateTime(2019, 2, 22, 14, 0, 0);
            Console.WriteLine(dt.ToString());
        }
        public void setMainModel(MainController.MainModel model)
        {
            this.mainModel = model;
            this.mainModel.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e)
               {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
        }
       
        //play speed
    }
}
