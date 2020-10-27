using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Model
{
    public class Zoom : INotifyPropertyChanged
    {
        /// <summary>
        /// Level of zoom, value <c>1</c> means no zoom at all.
        /// Value of this property must be greater or equal to 1.
        /// </summary>
        public double ZoomLevel
        {
            get => zoomLevel;
            set
            {
                zoomLevel = value;
                Changed(nameof(ZoomLevel));
                Changed(nameof(OffsetX));
                Changed(nameof(OffsetY));
            }
        }
        /// <summary>
        /// Horizontal offset of zoomed image.
        /// It is relative to ZoomLevel and image size, meaning that <c>OffsetX = 1</c> means maximum offset, when right edge of image overlaps with right edge of viewport.
        /// </summary>
        public double OffsetX
        {
            get => offsetX;
            set
            {
                offsetX = value;
                Changed(nameof(OffsetX));
            }
        }
        /// <summary>
        /// Vertical offset of zoomed image.
        /// It is relative to ZoomLevel and image size, meaning that <c>OffsetY = 1</c> means maximum offset, when bottom edge of image overlaps with bottom edge of viewport.
        /// </summary>
        public double OffsetY
        {
            get => offsetY;
            set
            {
                offsetY = value;
                Changed(nameof(OffsetY));
            }
        }

        private double zoomLevel = 1;
        private double offsetX = 0;
        private double offsetY = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        private void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
