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
        public CommandFunctions Commands { get; private set; }
        public MainWindow(Model.Model model)
        {
            Model = model;
            Commands = new CommandFunctions(this);
            InitializeComponent();
            InitKeyboardGestures();

            WindowState = WindowState.Maximized;
        }

        private Model.Model model;

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
            NextPage();
        }

        private void LeftPageChanger_Click(object sender, MouseButtonEventArgs e)
        {
            PreviousPage();
        }

        private void NextPage()
        {
            Model.SelectedTab?.NextPage();
        }
        private void PreviousPage()
        {
            Model.SelectedTab?.PreviousPage();
        }

        private void InitKeyboardGestures()
        {
            KeyGesture nextGesture = new KeyGesture(Key.Right);
            KeyGesture PrevGesture = new KeyGesture(Key.Left);

            LambdaCommand nextCommand = new LambdaCommand(t => NextPage());
            LambdaCommand prevCommand = new LambdaCommand(t => PreviousPage());

            InputBindings.Add(new InputBinding(nextCommand ,nextGesture));
            InputBindings.Add(new InputBinding(prevCommand ,PrevGesture));
        }
    }
}
