using System;
using System.Collections.Generic;
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


        public Tabs()
        {
            InitializeComponent();
        }
    }
}
