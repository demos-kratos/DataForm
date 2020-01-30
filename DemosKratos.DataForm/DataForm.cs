using DemosKratos.DataForm.Attributes;
using DemosKratos.DataForm.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace DemosKratos.DataForm
{
    public class DataForm : DataForm<RendererViewModel>
    {

    }

    public class DataForm<T> : ContentView where T : RendererViewModel, new()
    {        
        public BindableObject Source
        {
            get => (BindableObject)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(BindableObject), typeof(DataForm<T>), null, BindingMode.TwoWay, null, SourceChanged);

        public double SidePadding
        {
            get => (double)GetValue(SidePaddingProperty);
            set => SetValue(SidePaddingProperty, value);
        }
        public static BindableProperty SidePaddingProperty = BindableProperty.Create(nameof(SidePadding), typeof(double), typeof(DataForm<T>), 17.0);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DataForm<T>), "Форма");

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        public static BindableProperty StatusProperty = BindableProperty.Create(nameof(Status), typeof(string), typeof(DataForm<T>), "Статус");

        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty);
            set => SetValue(ShowHeaderProperty, value);
        }
        public static BindableProperty ShowHeaderProperty = BindableProperty.Create(nameof(ShowHeader), typeof(bool), typeof(DataForm<T>), true);

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }
        public static BindableProperty IsEditableProperty = BindableProperty.Create(nameof(IsEditable), typeof(bool), typeof(DataForm<T>), true);

        private Dictionary<string, RendererViewModel> RendererViewModels { get; } = new Dictionary<string, RendererViewModel>();
        private static Dictionary<Type, Func<PropertyInfo, T, ContentView>> Renderers { get; } = new Dictionary<Type, Func<PropertyInfo, T, ContentView>>() 
        { 
            { typeof(string), (p, vm) => p.GetCustomAttributes<LargeStringAttribute>(false).Any() ? (ContentView)new LargeStringRenderer() : new StringRenderer() },
            { typeof(DateTime), (p, vm) => p.GetCustomAttributes<DateOnlyAttribute>(false).Any() ? (ContentView)new DateRenderer() : new DateTimeRenderer() },
            { typeof(TimeSpan), (p, vm) => new TimeSpanRenderer() },
            { typeof(bool), (p, vm) => new BoolRenderer() },
            { typeof((string, string)), (p, vm) => { vm.Selector = p.GetCustomAttribute<OptionAttribute>(false)?.Selector; return new OptionRenderer(); } }
        };

        private Dictionary<string, System.ComponentModel.PropertyChangedEventHandler> EventHandlers { get; } = new Dictionary<string, System.ComponentModel.PropertyChangedEventHandler>();

        private List<Action> SettersQueue { get; } = new List<Action>();

        private List<string> HiddenProperties = new List<string>();

        private StackLayout Stack { get; set; }

        public DataForm()
        {
            Stack = new StackLayout { Margin = new Thickness(0, 7, 0, 0), Spacing = 0 };
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = Stack;
        }

        public void ResetSetters()
        {
            if(Source != null)
            {
                foreach (var h in EventHandlers)
                {
                    Source.PropertyChanged -= h.Value;
                }
            }
            
            EventHandlers.Clear();
        }

        public void SetEditable<TS>(string propertyName, Expression<Func<TS, bool>> expression)
        {
            if(Source == null)
            {
                SettersQueue.Add(() => SetEditableInternal(propertyName, expression));
            }
            else
            {
                SetEditableInternal(propertyName, expression);
            }
        }

        private void SetEditableInternal<TS>(string propertyName, Expression<Func<TS, bool>> expression)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name is empty or null", nameof(propertyName));
            }

            if (EventHandlers.ContainsKey(propertyName + "V"))
            {
                throw new InvalidOperationException("Can't have more than one enabled setters for a property. Call ResetSetters prior to this.");
            }

            if (!(Source is TS tSource))
            {
                throw new ArgumentException($"Type mismatch with the Source of the DataForm. Expected {Source.GetType().Name}, received {typeof(TS).Name}.", nameof(TS));
            }                

            var regex = new Regex(@$"{expression.Parameters[0].Name}\.(\w+)");
            var names = regex.Matches(expression.Body.ToString()).Select(x => x.Groups[1].Value).Distinct().Where(x => x != propertyName).ToList();
            var func = expression.Compile();

            System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) =>
            {
                if (names.Contains(e.PropertyName))
                    RendererViewModels[propertyName].IsEditable = func(tSource);
            };

            if(EventHandlers.ContainsKey(propertyName + "E"))
            {
                EventHandlers.Remove(propertyName + "E");
            }
            
            EventHandlers.Add(propertyName + "E", handler);
            Source.PropertyChanged += handler;
        }

        public void SetValue<TS, TV>(string propertyName, Expression<Func<TS, TV>> expression)
        {
            if (Source == null)
            {
                SettersQueue.Add(() => SetValueInternal(propertyName, expression));
            }
            else
            {
                SetValueInternal(propertyName, expression);
            }
        }

        private void SetValueInternal<TS, TV>(string propertyName, Expression<Func<TS, TV>> expression)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name is empty or null", nameof(propertyName));
            }

            if(EventHandlers.ContainsKey(propertyName + "V"))
            {
                throw new InvalidOperationException("Can't have more than one value setters for a property. Call ResetSetters prior to this.");
            }

            if (!(Source is TS tSource))
            {
                throw new ArgumentException("Type mismatch with the Source of the DataForm", nameof(TS));
            }

            if (!(RendererViewModels[propertyName].Value is TV))
            {
                throw new ArgumentException("Type mismatch with the Value of the Renderer", nameof(TS));
            }

            var regex = new Regex(@$"{expression.Parameters[0].Name}\.(\w+)");
            var names = regex.Matches(expression.Body.ToString()).Select(x => x.Groups[1].Value).Distinct().Where(x => x != propertyName).ToList();
            var func = expression.Compile();

            System.ComponentModel.PropertyChangedEventHandler handler = (sender, e) =>
            {
                if (names.Contains(e.PropertyName))
                    RendererViewModels[propertyName].Value = func(tSource);
            };

            if (EventHandlers.ContainsKey(propertyName + "V"))
            {
                EventHandlers.Remove(propertyName + "V");
            }

            EventHandlers.Add(propertyName + "V", handler);
            Source.PropertyChanged += handler;
        }

        public void Hide(string propertyName)
        {
            SetShowInternal(propertyName, false);
        }

        public void Show(string propertyName)
        {
            SetShowInternal(propertyName, true);
        }

        private void SetShowInternal(string propertyName, bool show)
        {
            if(show)
            {
                if(HiddenProperties.Contains(propertyName))
                {
                    HiddenProperties.Remove(propertyName);
                }                
            }
            else
            {
                if (!HiddenProperties.Contains(propertyName))
                {
                    HiddenProperties.Add(propertyName);
                }                
            }
            if(Source != null)
            {
                SourceChanged(this, null, Source);
            }
        }

        public static void AddRenderer(Type propertyType, Func<PropertyInfo, T, ContentView> rendererGetter)
        {
            if(Renderers.ContainsKey(propertyType))
            {
                var original = Renderers[propertyType];
                Func<PropertyInfo, T, ContentView> newGetter = (p, vm) =>
                {
                    var view = rendererGetter(p, vm);
                    if(view == null)
                    {
                        return original(p, vm);
                    }
                    else
                    {
                        return view;
                    }
                };
                Renderers.Remove(propertyType);
                Renderers.Add(propertyType, newGetter);
            }
            else
            {
                Renderers.Add(propertyType, rendererGetter);
            }            
        }

        private static void SourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(!(bindable is DataForm<T> df) || oldValue == newValue)
            {
                return;
            }

            df.Stack.Children.Clear();

            if(df.ShowHeader)
                PushHeader(df);

            var sourceType = newValue.GetType();
            var props = sourceType
                .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .Where(x => !x.GetCustomAttributes(false)
                    .OfType<IgnoreAttribute>()
                    .Any())
                .ToList();

            foreach (var p in props.Where(x => !df.HiddenProperties.Contains(x.Name)))
            {
                var attributes = p.GetCustomAttributes(false);
                var name = attributes.OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName;
                var placeholder = attributes.OfType<PlaceholderAttribute>().FirstOrDefault()?.Placeholder ?? "";
                var enabled = !attributes.OfType<DisableAttribute>().Any();
                var editable = !attributes.OfType<NotEditableAttribute>().Any() || df.IsEditable;
                var minvalprop = attributes.OfType<MinValueAttribute>().FirstOrDefault()?.PropertyName;
                var maxvalprop = attributes.OfType<MaxValueAttribute>().FirstOrDefault()?.PropertyName;

                var viewModel = new T();
                viewModel.SetParams(name, placeholder, editable, enabled, p.Name, df.SidePadding, newValue);

                if (!string.IsNullOrWhiteSpace(minvalprop))
                {
                    viewModel.MinValue = () => props.Where(x => x.Name == minvalprop).FirstOrDefault()?.GetValue(newValue);
                }

                if (!string.IsNullOrWhiteSpace(maxvalprop))
                {
                    viewModel.MaxValue = () => props.Where(x => x.Name == maxvalprop).FirstOrDefault()?.GetValue(newValue);
                }

                df.RendererViewModels[p.Name] = viewModel;

                ContentView renderer = Renderers.ContainsKey(p.PropertyType) ? Renderers[p.PropertyType](p, viewModel) : null;

                if (renderer != null)
                {
                    renderer.BindingContext = viewModel;
                    df.Stack.Children.Add(renderer);
                }
            }

            df.ResetSetters();
            foreach(var a in df.SettersQueue)
            {
                a();
            }
        }

        private static void PushHeader(DataForm<T> df)
        {
            var statusLabel = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                TextColor = (Color)Application.Current.Resources["Primary"]
            };
            statusLabel.SetBinding(Label.TextProperty, nameof(Status));

            var section = new Section
            {
                Control = statusLabel,
                BindingContext = df
            };
            section.SetBinding(Section.TextProperty, nameof(Title));
            section.SetBinding(Section.SidePaddingProperty, nameof(SidePadding));

            var box = new BoxView
            {
                Color = Color.FromHex("C7C7C7"),
                HeightRequest = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(df.SidePadding - 7, 0, df.SidePadding - 7, 7)
            };

            df.Stack.Children.Add(section);
            df.Stack.Children.Add(box);
        }

        public void Appearing()
        {
            foreach(var h in EventHandlers)
            {
                Source.PropertyChanged -= h.Value;
            }
        }

        public void Disappearing()
        {
            foreach(var h in EventHandlers)
            {
                Source.PropertyChanged += h.Value;
            }
        }
    }
}
