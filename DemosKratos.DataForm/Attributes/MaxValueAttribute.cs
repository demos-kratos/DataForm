using System;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MaxValueAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public MaxValueAttribute(string propName)
        {
            PropertyName = propName;
        }
    }
}
