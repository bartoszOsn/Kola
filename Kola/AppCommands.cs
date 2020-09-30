using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kola
{
    static public class AppCommands
    {
        public static readonly RoutedUICommand Open = new RoutedUICommand("Opens new tab", "Open", typeof(object));
    }
}
