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
using System.Windows.Shapes;

namespace Kola
{
    /*
     * setting are:
     *      How many pages in memory?
     *      Remember the page that closed tab was at
     *      Allow to open many tabs with same file
     *      
     * */

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
    }
}
