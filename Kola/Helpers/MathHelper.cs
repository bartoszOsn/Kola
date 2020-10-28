using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Helpers
{
    class MathHelper
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable
        {
            return (value.CompareTo(min) < 0) ? min : ((value.CompareTo(max) > 0) ? max : value);
        }
    }
}
