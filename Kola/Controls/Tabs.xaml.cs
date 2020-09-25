using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kola.Controls
{
    public struct StringEntry
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public bool Selected { get; set; }
    }
    public class StringArrayConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int selectedIndex = (int)values[1];
            return ((IEnumerable<string>)(values[0])).Select((t, i) => new StringEntry() { Text = t, Index = i, Selected = i == selectedIndex });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : UserControl
    {


        public IEnumerable<string> TabNames
        {
            get { return (IEnumerable<string>)GetValue(TabNamesProperty); }
            set { SetValue(TabNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TabNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabNamesProperty =
            DependencyProperty.Register("TabNames", typeof(IEnumerable<string>), typeof(Tabs), new PropertyMetadata(new string[] { "Hellblazer #341", "IZombie #1"}));



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Tabs), new PropertyMetadata(1));



        public Tabs()
        {
            InitializeComponent();
        }
    }
}
