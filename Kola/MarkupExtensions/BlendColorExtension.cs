using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace Kola.MarkupExtensions
{
    class BlendColorExtension : MarkupExtension
    {
        public SolidColorBrush A { get; set; }
        public SolidColorBrush B { get; set; }
        public float Mix { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Color A = this.A.Color;
            Color B = this.B.Color;
            Color Result = new Color();
            Result.ScR = A.ScR * Mix + B.ScR * (1 - Mix);
            Result.ScG = A.ScG * Mix + B.ScG * (1 - Mix);
            Result.ScB = A.ScB * Mix + B.ScB * (1 - Mix);
            Result.ScA = A.ScA * Mix + B.ScA * (1 - Mix);

            return Result;
        }
    }
}
