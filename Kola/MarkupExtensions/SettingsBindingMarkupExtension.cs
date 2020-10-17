using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kola.MarkupExtensions
{
    class SettingsBindingMarkupExtension : Binding, IValueConverter
    {
        public string IsInList
        {
            get => isInList;
            set
            {
                isInList = value;
                if(isInList == null)
                {
                    this.Converter = null;
                }
                else
                {
                    this.Converter = this;
                }
            }
        }

        private string isInList;
        private string list;

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

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            list = value as string;
            return doesContains();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(bool)value)
            {
                return string.Join(";", list.Split(';').Where(t => t != IsInList));
            }
            else
            {
                return doesContains() ? list : list + ";" + IsInList;
            }
        }

        private bool doesContains()
        {
            return list.Split(';').Contains(IsInList);
        }
    }
}
