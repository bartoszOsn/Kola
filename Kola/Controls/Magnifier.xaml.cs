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
    /// It should be direct child of canvas streched over whole area where magnifier can be used.
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
        }
    }
}
