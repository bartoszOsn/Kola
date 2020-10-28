using Kola.Helpers;
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
using System.Windows.Threading;

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

        private Model.Model model;
        private DispatcherTimer zoomDispatcher;
        private Vector zoomOffsetSpeed = new Vector(0, 0);
        private double zoomZoomSpeed = 0.0;

        public MainWindow(Model.Model model)
        {
            Model = model;
            Commands = new CommandFunctions(this);
            InitializeComponent();
            WindowState = WindowState.Maximized;
            zoomDispatcher = new DispatcherTimer();
            zoomDispatcher.Interval = TimeSpan.FromMilliseconds(5);
            zoomDispatcher.Tick += (s, e) =>
            {
                ProgressZoom();
            };
            zoomDispatcher.Start();
        }
        ~MainWindow()
        {
            zoomDispatcher.Stop();
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            KeyboardStateChanged(e);

            if(e.Key == Properties.Settings.Default.NextPageKey1 || e.Key == Properties.Settings.Default.NextPageKey2)
            {
                AppCommands.NextPage.Execute(null, this);
            }
            if (e.Key == Properties.Settings.Default.PreviousPageKey1 || e.Key == Properties.Settings.Default.PreviousPageKey2)
            {
                AppCommands.PreviousPage.Execute(null, this);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            KeyboardStateChanged(e);
        }

        private void KeyboardStateChanged(KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.W || e.Key == Key.S || e.Key == Key.D || e.Key == Key.R || e.Key == Key.F)
            {
                SetScroll();
            }
        }

        private void SetScroll()
        {
            zoomOffsetSpeed = new Vector(0, 0);
            zoomZoomSpeed = 0.0;
            zoomOffsetSpeed.X += Keyboard.IsKeyDown(Key.A) ? -0.1 : 0;
            zoomOffsetSpeed.X += Keyboard.IsKeyDown(Key.D) ? 0.1 : 0;
            zoomOffsetSpeed.Y += Keyboard.IsKeyDown(Key.W) ? -0.1 : 0;
            zoomOffsetSpeed.Y += Keyboard.IsKeyDown(Key.S) ? 0.1 : 0;

            zoomZoomSpeed += Keyboard.IsKeyDown(Key.F) ? -0.06 : 0;
            zoomZoomSpeed += Keyboard.IsKeyDown(Key.R) ? 0.06 : 0;

            ProgressZoom();
        }

        private void ProgressZoom()
        {
            if (model.SelectedTab?.Zoom != null)
            {
                model.SelectedTab.Zoom.OffsetX = MathHelper.Clamp(model.SelectedTab.Zoom.OffsetX + zoomOffsetSpeed.X / model.SelectedTab.Zoom.ZoomLevel, 0, 1);
                model.SelectedTab.Zoom.OffsetY = MathHelper.Clamp(model.SelectedTab.Zoom.OffsetY + zoomOffsetSpeed.Y / model.SelectedTab.Zoom.ZoomLevel, 0, 1);
                model.SelectedTab.Zoom.ZoomLevel = MathHelper.Clamp(model.SelectedTab.Zoom.ZoomLevel + zoomZoomSpeed, 1, 10);
            }
        }
    }
}
