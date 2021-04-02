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
        private DateTime dt;
        private int seconds; 
        private string t;
        public string VM_Speed
        {
            get { return mainModel.Speed; }
            set { mainModel.Speed = value; }
        }
        public double VM_Size
        {
            get { return mainModel.Size / 10; }
        }
        public double VM_CurrentLine
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
                int h = seconds / 3600;
                seconds = seconds - h * 3600;
                int m = seconds / 60;
                int s = seconds - m * 60;
                string hour, min, sec, str;
                if (h < 10)
                    hour = $"0{h.ToString()}";
                else
                    hour = $"{h.ToString()}";
                if (m < 10)
                    min = $"0{m.ToString()}";
                else
                    min = $"{m.ToString()}";
                if (s < 10)
                    sec = $"0{s.ToString()}";
                else
                    sec = $"{s.ToString()}";
                str = $"{hour}:{min}:{sec}";
                return str;
            }
            set
            {
               
            }
            
        }
        private MainController.MainModel mainModel;
        public videoControlViewModel()
        {
            
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
        public void pauseVideo()
        {
            mainModel.pauseVideo();
        }
        public void playVideo()
        {
            mainModel.playVideo();
        }
        public void stopVideo()
        {
            mainModel.stopVideo();
        }
        public void forwardVideo(int sec)
        {
            mainModel.forwardVideo(sec);
        }
        public void backVideo(int sec)
        {
            mainModel.backVideo(sec);
        }
        public void prevVideo()
        {
            mainModel.prevVideo();
        }
        public void nextVideo()
        {
            mainModel.nextVideo();
        }
        //play speed
    }
}
