using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Kola
{
    class FloatToGridLengthConverter : IValueConverter
    {
        /// <summary>
        /// Converts float value to the same number of stars.
        /// </summary>
        /// <param name="value">Number of stars</param>
        /// <param name="targetType">Type of Object that it chould be converted to</param>
        /// <param name="parameter">Should value be inverted, i.e. instead of (value)* should it return (1 - value)*?</param>
        /// <param name="culture">Culture info</param>
        /// <returns>Return instance of GridLength</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float v = (float)value;
            if(parameter is bool && (bool)parameter)
            {
                v = 1.0f - v;
            }
            return new GridLength(v, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
