using System;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NotEditableAttribute : Attribute
    {
        public NotEditableAttribute()
        {

        }
    }
}
