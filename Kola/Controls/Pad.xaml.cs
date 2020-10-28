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
    /// Interaction logic for Pad.xaml
    /// </summary>
    public partial class Pad : UserControl
    {

        /// <summary>
        /// How times smaller the inner rectangle is?
        /// </summary>
        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Zoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(Pad), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));


        /// <summary>
        /// Value between 0 and 1, 0 means touthing left edge, while 1 means touching right edge.
        /// </summary>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffsetX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register("OffsetX", typeof(double), typeof(Pad), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// Value between 0 and 1, 0 means touthing top edge, while 1 means touching bottom edge.
        /// </summary>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffsetY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register("OffsetY", typeof(double), typeof(Pad), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // Using a DependencyProperty as the backing store for ScreenHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScreenHeightProperty =
            DependencyProperty.Register("ScreenHeight", typeof(double), typeof(Pad), new PropertyMetadata(0.0));




        public Pad()
        {
            InitializeComponent();
        }

        private void AlignOffset(Point mousePos)
        {
            var a1 = (root.ActualWidth - image.ActualWidth) / 2;
            var a2 = (root.ActualHeight - image.ActualHeight) / 2;

            var b1 = image.ActualWidth + (root.ActualWidth - image.ActualWidth) / Zoom;
            var b2 = image.ActualHeight + (root.ActualHeight - image.ActualHeight) / Zoom;
            mousePos.X -= a1 - a1 / Zoom;
            mousePos.Y -= a2 - a2 / Zoom;


            double innerWidth = root.ActualWidth / Zoom;
            double innerHeight = root.ActualHeight / Zoom;

            double x = (mousePos.X - innerWidth / 2.0) / (b1 - innerWidth);
            double y = (mousePos.Y - innerHeight / 2.0) / (b2 - innerHeight);

            x = Helpers.MathHelper.Clamp(x, 0, 1);
            y = Helpers.MathHelper.Clamp(y, 0, 1);

            OffsetX = x;
            OffsetY = y;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            CaptureMouse();
            AlignOffset(Mouse.GetPosition(root));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                AlignOffset(Mouse.GetPosition(root));
        }
    }
}
