using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// User control, that works similarly to slider, but when clicked on the track it instantly sets the value according to clicked place, instead of moving value just a little bit towards cursor.
    /// </summary>
    public partial class TrackBar : UserControl, INotifyPropertyChanged
    {
        public TrackBar()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Minimum possible value.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(TrackBar), new PropertyMetadata(0, OnPropertyChange));


        /// <summary>
        /// Maximum possible value.
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(TrackBar), new PropertyMetadata(1, OnPropertyChange));


        /// <summary>
        /// Value pointed at by thumb.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(TrackBar), new PropertyMetadata(0, OnPropertyChange));


        
        public int ThumbWidth
        {
            get { return (int)GetValue(ThumbWidthProperty); }
            set { SetValue(ThumbWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof(int), typeof(TrackBar), new PropertyMetadata(0));



        /// <summary>
        /// Normalised value, where 0 means MinimumValue and 1 means MaximumValue.
        /// </summary>
        public float NormalisedValue
        {
            get
            {
                if(Minimum == Maximum)
                {
                    return 0.0f;
                }
                return (float)(Value - Minimum) / (Maximum - Minimum);
            }
            set
            {
                Value = (int)Math.Round(value * (Maximum - Minimum) + Minimum);
            }
        }

        public bool IsBeingDragged
        {
            get { return isBeingDragged; }
            set
            {
                isBeingDragged = value;
                Changed(nameof(IsBeingDragged));
            }
        }

        /// <summary>
        /// Event that is fired when some property changes its value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBeingDragged = false;

        private static void OnPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TrackBar)d).Changed(nameof(NormalisedValue));
        }

        /// <summary>
        /// Notifies That property changed.
        /// </summary>
        /// <param name="name">Name of changed property.</param>
        private void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsBeingDragged = true;

            CaptureMouse();

            OnMouseMove(this, e);
        }

        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsBeingDragged = false;

            ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(IsBeingDragged)
            {
                double relativePoint = e.GetPosition(grid).X;
                NormalisedValue = (float)(relativePoint / grid.ActualWidth);
            }
            
        }
    }
}
