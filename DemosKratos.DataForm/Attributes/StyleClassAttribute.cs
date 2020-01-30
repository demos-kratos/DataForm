using System;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StyleClassAttribute : Attribute
    {
        public string StyleClass { get; set; }

        public StyleClassAttribute(string styleClass)
        {
            StyleClass = styleClass;
        }
    }
}
