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
    /// Control which magnifies part of image control.
    /// </summary>
    /// <remarks>
    /// It should be direct child of canvas with background property set and streched over whole area where magnifier can be used.
    /// It appears when you click on point on canvas and then drag mouse, to change it's size.
    /// While holding left mouse button, you can click right mouse button and drag to change magnification factor.
    /// </remarks>
    public partial class Magnifier : UserControl
    {

        /// <summary>
        /// Image that this control will magnify.
        /// </summary>
        public Image Image
        {
            get { return (Image)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Image), typeof(Magnifier));

        /// <summary>
        /// Shortcut for casting parent to Canvas.
        /// </summary>
        public Canvas Canvas
        {
            get => Parent as Canvas;
        }

        public Magnifier()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        protected void OnLoaded(object sender, RoutedEventArgs e)
        {
            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseUp += Canvas_MouseDown;
            Canvas.MouseMove += Canvas_MouseMove;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ButtonState == MouseButtonState.Pressed)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    MouseLeftDown();
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    MouseRightDown();
                }
            }
            else
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    MouseLeftUp();
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    MouseRightUp();
                }
            }
            
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(leftClick)
            {
                if(rightClick)
                {
                    ResetZoom();
                }
                else
                {
                    ResetRadius();
                }
            }
        }

        private bool leftClick = false;
        private bool rightClick = false;
        private Point lectClickCenter;
        private Point RightClickCenter;
        private double factor = 2.0;

        private double getDistance(Point p1, Point p2)
        {
            Vector v = new Vector(p1.X - p2.X, p1.Y - p2.Y);
            return v.Length;
        }

        private void MouseLeftDown()
        {
            Canvas.CaptureMouse();
            lectClickCenter = Mouse.GetPosition(Canvas);
            leftClick = true;
        }

        private void MouseLeftUp()
        {
            Canvas.ReleaseMouseCapture();
            leftClick = false;
            Hide();
        }

        private void MouseRightDown()
        {
            RightClickCenter = Mouse.GetPosition(Canvas);
            rightClick = true;
        }

        private void MouseRightUp()
        {
            rightClick = false;
        }

        private double GetRadius()
        {
            Point mouse = Mouse.GetPosition(Canvas);
            return getDistance(mouse, lectClickCenter);
        }
        
        private void PositionImage(double radius)
        {
            Vector imageSize = GetRealImageSize();
            Vector scale = GetImageScale();
            InternalImage.Width = factor * imageSize.X;
            InternalImage.Height = factor * imageSize.Y;

            Point relativeLeftClickCenter = lectClickCenter;

            Point CenterInImage = Canvas.TranslatePoint(relativeLeftClickCenter, Image);

            Canvas.SetLeft(InternalImage, radius - CenterInImage.X * factor * scale.X);
            Canvas.SetTop(InternalImage, radius - CenterInImage.Y * factor * scale.Y);
        }
        private void ResetRadius()
        {
            double radius = GetRadius();
            Width = 2 * radius;
            Height = 2 * radius;

            Canvas.SetLeft(this, lectClickCenter.X - radius);
            Canvas.SetTop(this, lectClickCenter.Y - radius);

            ClipCircle.RadiusX = radius;
            ClipCircle.RadiusY = radius;
            ClipCircle.Center = new Point(radius, radius);

            PositionImage(radius);
        }

        private void ResetZoom()
        {
            Point mouse = Mouse.GetPosition(Canvas);

            double distToLeft = getDistance(mouse, lectClickCenter);
            double distToRight = getDistance(mouse, RightClickCenter);
            double distFromLeftToRignt = getDistance(lectClickCenter, RightClickCenter);

            distToLeft /= distFromLeftToRignt;
            distToRight /= distFromLeftToRignt;

            factor = 2 * distToLeft + distToRight;

            PositionImage(distFromLeftToRignt);
        }

        private void Hide()
        {
            ClipCircle.RadiusX = 0;
            ClipCircle.RadiusY = 0;
            factor = 2;
        }

        private Vector GetRealImageSize()
        {
            Vector v = GetImageScale();
            v.X = Image.ActualWidth * v.X;
            v.Y = Image.ActualHeight * v.Y;
            return v;
        }

        private Vector GetImageScale()
        {
            TransformGroup group = Image.RenderTransform as TransformGroup;
            ScaleTransform scaleTransform = group.Children.First(t => t is ScaleTransform) as ScaleTransform;
            return new Vector(scaleTransform.ScaleX, scaleTransform.ScaleY);
        }
    }
}
