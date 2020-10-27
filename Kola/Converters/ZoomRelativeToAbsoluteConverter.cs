using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Kola.Converters
{
    enum ZoomProperty
    {
        OffsetX,
        OffsetY
    }

    /// <summary>
    /// Converts relative values from model to useful absolute values in pixels.
    /// Bindings should be assigned in this order:
    /// <list type="number">
    ///     <item>property from <c>Zoom</c> model</item>
    ///     <item>Image control</item>
    ///     <item>ZoomProperty describing which property we want to get.</item>
    /// </list>
    /// </summary>
    class ZoomRelativeToAbsoluteConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts given values to expected property
        /// </summary>
        /// <param name="values">
        ///     It should be following values:
        ///     <list type="number">
        ///         <item>property from <c>Zoom</c> model</item>
        ///         <item>Image control</item>
        ///         <item>ZoomProperty describing which property we want to get.</item>
        ///     </list>
        /// </param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length < 3)
            {
                return null;
            }
            double prop = (double)values[0];
            Image image = values[1] as Image;
            ZoomProperty zoomProperty = (ZoomProperty)values[2];

            switch(zoomProperty)
            {
                case ZoomProperty.OffsetX:
                    return GetOffsetX(prop, image);
                case ZoomProperty.OffsetY:
                    return GetOffsetY(prop, image);
            }
            return null;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns real offset in pixels.
        /// </summary>
        private double GetOffsetX(double offsetX, Image image)
        {
            double width = GetRealWidth(image);
            double viewportWidth = image.ActualWidth;
            if(width <= viewportWidth)
            {
                return 0;
            }
            return (width - viewportWidth) * offsetX;
        }

        /// <summary>
        /// Returns real offset in pixels.
        /// </summary>
        private double GetOffsetY(double offsetY, Image image)
        {
            double width = GetRealWidth(image);
            double viewportWidth = image.ActualWidth;
            if (width <= viewportWidth)
            {
                return 0;
            }
            return (width - viewportWidth) * offsetY;
        }

        /// <summary>
        /// Returns real width of image, after applying ScaleTransform.
        /// </summary>
        private double GetRealWidth(Image img)
        {
            TransformGroup group = img.RenderTransform as TransformGroup;
            return ((ScaleTransform)group.Children.First(t => t is ScaleTransform)).ScaleX * img.ActualWidth;
        }


        /// <summary>
        /// Returns real height of image, after applying ScaleTransform.
        /// </summary>
        private double GetRealHeight(Image img)
        {
            TransformGroup group = img.RenderTransform as TransformGroup;
            return ((ScaleTransform)group.Children.First(t => t is ScaleTransform)).ScaleY * img.ActualHeight;
        }
    }
}
