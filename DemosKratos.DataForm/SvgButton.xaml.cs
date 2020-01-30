using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemosKratos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SvgButton : ContentView
    {
        public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(SvgButton));
        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SvgButton));
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SvgButton));
        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SvgButton));
        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SvgButton));

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
                
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public ICommand Command 
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public event EventHandler Clicked;

        public SvgButton()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if(Command != null && Command.CanExecute(null))
            {
                Command.Execute(null);
                Clicked?.Invoke(this, new EventArgs());
            }
            
        }
    }
}