﻿using System;
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



        public Pad()
        {
            InitializeComponent();
        }

        private double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : ((value > max) ? max : value);
        }

        private void AlignOffset(Point mousePos)
        {
            double innerWidth = ActualWidth / Zoom;
            double innerHeight = ActualHeight / Zoom;

            double x = (mousePos.X - innerWidth / 2.0) / (ActualWidth - innerWidth);
            double y = (mousePos.Y - innerHeight / 2.0) / (ActualHeight - innerHeight);

            x = Clamp(x, 0, 1);
            y = Clamp(y, 0, 1);

            OffsetX = x;
            OffsetY = y;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            CaptureMouse();
            AlignOffset(Mouse.GetPosition(this));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                AlignOffset(Mouse.GetPosition(this));
        }
    }
}
