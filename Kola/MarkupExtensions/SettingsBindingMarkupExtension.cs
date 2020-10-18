using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kola.MarkupExtensions
{
    class SettingsBindingMarkupExtension : Binding
    {

        public SettingsBindingMarkupExtension()
        {
            Initialise();
        }

        public SettingsBindingMarkupExtension(string path) : base(path)
        {
            Initialise();
        }

        private void Initialise()
        {
            this.Source = Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
