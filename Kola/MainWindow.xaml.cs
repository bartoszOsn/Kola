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

        /// <summary>
        /// Width of the area, where comic page is displayed.
        /// </summary>
        public static double GetViewportWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(ViewportWidthProperty);
        }

        public static void SetViewportWidth(DependencyObject obj, double value)
        {
            obj.SetValue(ViewportWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewportWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewportWidthProperty =
            DependencyProperty.RegisterAttached("ViewportWidth", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

        /// <summary>
        /// Height of the area, where comic page is displayed.
        /// </summary>
        public static double GetViewportHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(ViewportHeightProperty);
        }

        public static void SetViewportHeight(DependencyObject obj, double value)
        {
            obj.SetValue(ViewportHeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewportHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewportHeightProperty =
            DependencyProperty.RegisterAttached("ViewportHeight", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));





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
            InputBindings.Add(new InputBinding(AppCommands.NextPage , new KeyGesture(Key.Right)));
            InputBindings.Add(new InputBinding(AppCommands.PreviousPage , new KeyGesture(Key.Left)));
        }
    }
}
