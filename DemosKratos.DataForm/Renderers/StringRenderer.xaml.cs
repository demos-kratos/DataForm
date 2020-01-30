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
    public partial class StringRenderer : ContentView
    {
        public StringRenderer()
        {
            InitializeComponent();
        }
    }
}