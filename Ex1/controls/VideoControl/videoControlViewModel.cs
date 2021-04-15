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
        private double vm_videoLength;
        private double vm_currentTime;
        public double VM_Speed
        {
            get { return vm.Speed; }
            set { vm.Speed = value; }
        }
        public int VM_VideoLength
        {
            get { return (int)this.vm_videoLength; }
            set
            {
                this.vm_videoLength = value;
                NotifyPropertyChanged("VM_VideoLength");
            }
        }
        public double CurrentTimeChange
        {
            set
            {
                if (this.vm != null)
                {
                    int newTime = (int)(value - this.vm_currentTime);
                    if (newTime > 0)
                        forwardVideo(newTime);
                    else
                        backVideo(Math.Abs(newTime));
                }
            }
        }
        public double VM_CurrentTime
        {
            get { return this.vm_currentTime; }
            set
            {
                this.vm_currentTime = value;
                NotifyPropertyChanged("VM_CurrentTime");
                NotifyPropertyChanged("VM_Time");
            }
        }
        public string VM_Time
        {
            get
            { //cast seconds to display time in string.
                int seconds = (int)VM_CurrentTime;
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
            this.VM_VideoLength = 1;
            this.VM_CurrentTime = 0;

        }
        public override void setMainViewModel(MainViewModel vm)
        {
            base.setMainViewModel(vm);
            this.VM_VideoLength = vm.Size / vm.Frequency; //calculate video length.
            this.vm.PropertyChanged += //listen to MainViewModel.
               delegate (Object sender, PropertyChangedEventArgs e)
               {
                   if (e.PropertyName.Equals("LineNumber"))
                       this.VM_CurrentTime = vm.LineNumber / vm.Frequency;
                   else if (e.PropertyName.Equals("Size"))
                       this.VM_VideoLength = vm.Size / vm.Frequency;
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
        }
        public void pauseVideo()
        {
            this.vm.stopReading();
        }
        public void playVideo()
        {
            this.vm.startReading();
        }
        public void stopVideo()
        {
            this.vm.stopReading();
            this.vm.LineNumber = 0;
        }
        public void forwardVideo(int sec)
        {
            this.vm.skipForward(sec);
        }
        public void backVideo(int sec)
        {
            this.vm.skipBackwards(sec);
        }
        public void prevVideo()
        {
            stopVideo();
            playVideo();
        }
        public void nextVideo()
        {
            pauseVideo();
            vm.LineNumber = vm.Size - 1;
        }
    }
}
