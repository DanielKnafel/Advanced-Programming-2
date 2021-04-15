using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ex1.controls.Dashboard
{
    class KnotsToAnglesConverter : IValueConverter
    {
        //convert from knots to angles
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 360 / 160;
        }

        //convert from angles to knots
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
