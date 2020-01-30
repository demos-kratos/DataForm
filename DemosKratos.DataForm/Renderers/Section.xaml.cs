using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemosKratos.DataForm.Renderers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Section : ContentView
    {
        public double SidePadding
        {
            get => (double)GetValue(SidePaddingProperty);
            set => SetValue(SidePaddingProperty, value);
        }
        public static BindableProperty SidePaddingProperty = BindableProperty.Create(nameof(SidePadding), typeof(double), typeof(Section));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(Section));

        public View Control
        {
            get => (View)GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }
        public static BindableProperty ControlProperty = BindableProperty.Create(nameof(Control), typeof(View), typeof(Section));

        public Color DisabledColor
        {
            get => (Color)GetValue(DisabledColorProperty);
            set => SetValue(DisabledColorProperty, value);
        }
        public static BindableProperty DisabledColorProperty = BindableProperty.Create(nameof(DisabledColor), typeof(Color), typeof(Section), Color.LightGray);

        public Section()
        {
            InitializeComponent();
        }
    }
}