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
    public partial class DateRenderer : ContentView
    {
        private IDialogService Dialogs => DependencyService.Get<IDialogService>();
        private bool Once = true;
        private RendererViewModel ViewModel => BindingContext as RendererViewModel;

        public DateRenderer()
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
            if (sender is RendererViewModel vm && vm.Value is DateTime dt && e.PropertyName == nameof(RendererViewModel.Value) && dt != DateTime.MinValue)
            {
                DateEntry.Text = dt.ToShortDateString();
                DateLabel.Text = dt.ToShortDateString();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (ViewModel.IsEditable && ViewModel.IsEnabled && ViewModel.Value is DateTime dt)
            {
                var val = await Dialogs.GetDate(null, dt == DateTime.MinValue ? (DateTime?)null : dt);
                if (val != null)
                {
                    ViewModel.Value = val.Value;
                }
            }
        }
    }
}