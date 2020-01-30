using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;

namespace DemosKratos.DataForm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    sealed public class DisableAttribute : Attribute
    {
        public DisableAttribute()
        {

        }
    }
}
