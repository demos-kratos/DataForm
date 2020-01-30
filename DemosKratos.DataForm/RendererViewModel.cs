using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DemosKratos.DataForm.Renderers
{
    public class RendererViewModel : BindableObject
    {
        public object Value
        {
            get => GetValue(ValueProperty);
            set { SetValue(ValueProperty, value); OnPropertyChanged(); }
        }
        public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(RendererViewModel), null, BindingMode.TwoWay, null, ValueChanged);

        public Func<object> MaxValue { get; set; }
        public Func<object> MinValue { get; set; }

        private static void ValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(oldValue != newValue && bindable is RendererViewModel vm)
            {
                vm.OnPropertyChanged(nameof(Value));
                vm.OnPropertyChanged(nameof(IsOptionSelected));
            }
        }

        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                if (_DisplayName == value)
                    return;
                _DisplayName = value;
                OnPropertyChanged();
            }
        }
        private string _DisplayName;

        public string Placeholder
        {
            get => _Placeholder;
            set
            {
                if (_Placeholder == value)
                    return;
                _Placeholder = value;
                OnPropertyChanged();
            }
        }
        private string _Placeholder;

        public bool IsEditable
        {
            get => _Editable;
            set
            {
                if (_Editable == value)
                    return;
                _Editable = value;
                OnPropertyChanged();
            }
        }
        private bool _Editable;

        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                if (_IsEnabled == value)
                    return;
                _IsEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool _IsEnabled;

        public double SidePadding
        {
            get => _SidePadding;
            set
            {
                if (_SidePadding == value)
                    return;
                _SidePadding = value;
                OnPropertyChanged();
            }
        }
        private double _SidePadding;

        public Func<Task<(string, string)>> Selector { get; set; }

        public bool IsOptionSelected => Value is ValueTuple<string, string> t && t.Item2 != null;

        public ICommand SelectCommand
        {
            get => _SelectCommand;
            set
            {
                if (_SelectCommand == value)
                    return;
                _SelectCommand = value;
                OnPropertyChanged();
            }
        }
        private ICommand _SelectCommand;

        public RendererViewModel()
        {
            
        }

        public void SetParams(string displayName, string placeholder, bool editable, bool enabled, string propertyName, double sidePadding, object context)
        {
            DisplayName = displayName;
            Placeholder = placeholder;
            IsEditable = editable;
            IsEnabled = enabled;
            SidePadding = sidePadding;
            SelectCommand = new Command(SelectOption);
            BindingContext = context;
            this.SetBinding(ValueProperty, propertyName);
        }

        private async void SelectOption()
        {
            if (!(Value is ValueTuple<string,string>))
                return;
            Value = await Selector();
        }
    }
}
