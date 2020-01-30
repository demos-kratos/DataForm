using DemosKratos.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemosKratos.DataForm.Renderers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSpanRenderer : ContentView
    {
        private IDialogService Dialogs => DependencyService.Get<IDialogService>();
        private RendererViewModel ViewModel => BindingContext as RendererViewModel;
        private bool Once = true;

        public TimeSpanRenderer()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            if (BindingContext is RendererViewModel && Once)
            {
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
                ViewModel_PropertyChanged(ViewModel, new PropertyChangedEventArgs(nameof(RendererViewModel.Value)));
                Once = false;
            }
            else
            {
                Once = true;
            }
            base.OnBindingContextChanged();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is RendererViewModel vm && vm.Value is TimeSpan dt && e.PropertyName == nameof(RendererViewModel.Value))
            {
                TimeEntry.Text = dt.ToString("hh\\:mm");
                TimeLabel.Text = dt.ToString("hh\\:mm");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (ViewModel.IsEditable && ViewModel.IsEnabled && ViewModel.Value is TimeSpan dt)
            {
                var val = await Dialogs.GetTime(null, dt);
                var valid = val != null;
                if(valid && ViewModel.MinValue != null && ViewModel.MinValue() is TimeSpan tsmin)
                {
                    valid &= val > tsmin;
                }
                if(valid && ViewModel.MaxValue != null && ViewModel.MaxValue() is TimeSpan tsmax)
                {
                    valid &= val < tsmax;
                }
                if (valid)
                {
                    ViewModel.Value = val;
                }
            }
        }
    }
}