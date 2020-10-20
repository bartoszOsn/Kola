using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Helpers
{
    public static class ColorExcs
    {
        public static string ToCSSString(this Color color)
        {
            return $"#{color.R:X2}{color.B:X2}{color.G:X2}";
        }
    }
}
