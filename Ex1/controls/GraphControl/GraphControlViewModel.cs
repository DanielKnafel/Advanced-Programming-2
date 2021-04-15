using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1.controls
{
    class GraphControlViewModel : ViewModel
    {
        public GraphControlViewModel()
        {

        }
        public override void setMainViewModel(MainViewModel vm)
        {
            base.setMainViewModel(vm);
            this.vm.PropertyChanged +=
                    delegate (Object sender, PropertyChangedEventArgs e)
                    {
                        if (e.PropertyName.Equals("LineNumber"))
                        {
                            NotifyPropertyChanged("VM_LineNumber");
                        }
                    };
        }

        public int LineNumber
        {
            get { return this.vm.LineNumber; }
        }
    }
}
