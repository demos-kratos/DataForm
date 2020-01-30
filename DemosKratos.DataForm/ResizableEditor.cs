using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DemosKratos
{
    public class ResizableEditor : Editor
    {
        public ResizableEditor() : base()
        {
            TextChanged += ResizableEditor_TextChanged;
        }

        private void ResizableEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure(); 
        }
    }
}
