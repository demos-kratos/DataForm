using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DemosKratos.Helpers
{
    public class BoolColorConverter : BindableObject, IValueConverter
    {
        public Color TrueColor
        {
            get => (Color)GetValue(TrueColorProperty);
            set => SetValue(TrueColorProperty, value);
        }
        public static BindableProperty TrueColorProperty = BindableProperty.Create(nameof(TrueColor), typeof(Color), typeof(BoolColorConverter));

        public Color FalseColor
        {
            get => (Color)GetValue(FalseColorProperty);
            set => SetValue(FalseColorProperty, value);
        }
        public static BindableProperty FalseColorProperty = BindableProperty.Create(nameof(FalseColor), typeof(Color), typeof(BoolColorConverter));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueColor : FalseColor; //Application.Current.Resources["Contrary"] : Application.Current.Resources["Primary"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var color = (Color)value;
                return color.A == TrueColor.A &&
                    color.R == TrueColor.R &&
                    color.G == TrueColor.G &&
                    color.B == TrueColor.B;
            }
            catch
            {
                return false;
            }
        }
    }
}
