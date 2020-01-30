using System;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    sealed public class PlaceholderAttribute : Attribute
    {
        public string Placeholder { get; set; }

        public PlaceholderAttribute(string placeholder)
        {
            Placeholder = placeholder;
        }
    }
}
