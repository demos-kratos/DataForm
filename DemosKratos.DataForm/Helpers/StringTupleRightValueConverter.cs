using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DemosKratos.Helpers
{
    public class StringTupleRightValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ValueTuple<string, string> t))
                throw new ArgumentException("Only ValueTuple<string, string> is supported in StringTupleRightValueConverter");

            return t.Item2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
