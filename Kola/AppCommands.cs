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
        public static readonly RoutedUICommand OpenTab = new RoutedUICommand("Opens new tab", "OpenTab", typeof(object));
        public static readonly RoutedUICommand SelectTab = new RoutedUICommand("Selects tab", "SelectTab", typeof(object));
        public static readonly RoutedUICommand CloseTab = new RoutedUICommand("Closes tab", "CloseTab", typeof(object));
        public static readonly RoutedUICommand OpenSettings = new RoutedUICommand("Opens Settings", "OpenSettings", typeof(object));
        public static readonly RoutedUICommand OpenDictionary = new RoutedUICommand("Opens Dictionary Popup", "OpenDictionary", typeof(object));
        public static readonly RoutedUICommand NextPage = new RoutedUICommand("Moves document forward by one page.", "NextPage", typeof(object));
        public static readonly RoutedUICommand PreviousPage = new RoutedUICommand("Moves document backward by one page.", "PreviousPage", typeof(object));
        public static readonly RoutedUICommand CloseWindow = new RoutedUICommand("Closes window.", "CloseWindow", typeof(object));
        public static readonly RoutedUICommand MaximiseWindow = new RoutedUICommand("Maximizes window.", "MaximiseWindow", typeof(object));
        public static readonly RoutedUICommand RestoreWindow = new RoutedUICommand("Restores window.", "RestoreWindow", typeof(object));
        public static readonly RoutedUICommand MinizeWindow = new RoutedUICommand("Minimize window.", "MinizeWindow", typeof(object));
    }
}
