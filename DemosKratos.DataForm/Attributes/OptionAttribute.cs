using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using DemosKratos.Services;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    sealed public class OptionAttribute : Attribute
    {
        public Func<Task<(string, string)>> Selector { get; set; }

        public OptionAttribute(Type selectorType)
        {
            var selectorObj = Activator.CreateInstance(selectorType);
            if(selectorObj is IOptionSelector s)
            {
                Selector = async () => await s.Select();
            }
            else
            {
                throw new ArgumentException("selector Type must implement IOptionSelector");
            }
        }

        public OptionAttribute(string[] options)
        {
            Selector = async () => {
                var selection = await DependencyService.Get<IDialogService>().Sheet(new DialogSheetInfo { Cancel = "Отмена", Items = options });
                return (Array.IndexOf(options, selection).ToString(), selection);
            };
        }
    }

    public interface IOptionSelector
    {
        Task<(string, string)> Select();
    }
}
