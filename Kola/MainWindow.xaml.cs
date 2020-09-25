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

namespace Kola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model.Model Model
        {
            get => model;
            set
            {
                model = value;
                DataContext = value;
            }
        }
        public MainWindow(Model.Model model)
        {
            Model = model;
            InitializeComponent();
        }

        private Model.Model model;

        private void Tabs_OnNewTab(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowDialog(this);
            Model.Add(dialog.FileNames);
        }

        private void Tabs_OnCloseTab(object sender, Controls.TabEventArgs e)
        {
            Model.Close(e.Index);
        }

        private void Tabs_OnSelectTab(object sender, Controls.TabEventArgs e)
        {
            Model.SelectedTabIndex = e.Index;
        }

        private void RightPageChanger_Click(object sender, MouseButtonEventArgs e)
        {
            Model.SelectedTab.NextPage();
        }

        private void LeftPageChanger_Click(object sender, MouseButtonEventArgs e)
        {
            Model.SelectedTab.PreviousPage();
        }
    }
}
