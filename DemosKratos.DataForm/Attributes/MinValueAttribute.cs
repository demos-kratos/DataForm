using System;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MinValueAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public MinValueAttribute(string propName)
        {
            PropertyName = propName;
        }
    }
}
