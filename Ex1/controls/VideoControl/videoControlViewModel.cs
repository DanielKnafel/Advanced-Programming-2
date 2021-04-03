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
        private int seconds; 
        private DataFileReader reader;
        public double VM_Speed
        {
            get { return reader.Speed; }
            set { reader.Speed = value; }
        }
        public double VM_VideoLength
        {
            get { return reader.Size / reader.Frequency; }
        }
        public double VM_CurrentTime
        {
            get
            {
                return this.reader.LineNumber / reader.Frequency;
            }
        }
        public string VM_Time
        {
            get
            {
                seconds = (int)Math.Round(VM_CurrentTime);
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
        }
        public videoControlViewModel()
        {
            this.seconds = 0;
        }
        public void setDataFileReader(DataFileReader reader)
        {
            this.reader = reader;
            this.reader.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e)
               {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
        }
        public void pauseVideo()
        {
            this.reader.stopReading();
        }
        public void playVideo()
        {
            this.reader.startReading();
        }
        public void stopVideo()
        {
            this.reader.stopReading();
            this.reader.LineNumber = 0;
        }
        public void forwardVideo(int sec)
        {
            this.reader.skipForward(sec);
        }
        public void backVideo(int sec)
        {
            this.reader.skipBackwards(sec);
        }
        public void prevVideo()
        {
            stopVideo();
            playVideo();
        }
        public void nextVideo()
        {
            pauseVideo();
            reader.LineNumber = reader.Size - 1;
        }
    }
}
