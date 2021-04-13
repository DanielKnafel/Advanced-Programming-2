using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex1.controls.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private DashboardViewModel vm;

        // thousands of ft
        private double altitude;

        // Km in a hour
        private double airspeed;

        //degrees
        private double pitch;
        private double roll;
        private double yaw;
        private double direction;


        public Dashboard()
        {
            InitializeComponent();
            this.vm = new DashboardViewModel();
            DataContext = vm;
        }

        public void setMainViewModel(MainViewModel vm)
        {
            this.vm.setMainViewModel(vm);
        }

    }
}
