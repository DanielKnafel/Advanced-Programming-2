using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace Ex1.controls
{
    class videoControlViewModel:ViewModel
    {
        private TimeSpan time;
        public TimeSpan VM_Time
        {
            get { return time; }
            set
            {
                time = value;
                //model
            }
        }
        public videoControlViewModel()
        {
            
        }
        //play speed
    }
}
