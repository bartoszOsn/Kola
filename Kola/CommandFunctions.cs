using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kola
{
    public class CommandFunctions
    {
        public MainWindow Window { get; private set; }

        public CommandFunctions(MainWindow window)
        {
            Window = window;
            SetUpBindings();
        }

        public void Open(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowDialog(Window);
            Window.Model.Add(dialog.FileNames);
            Window.CommandBindings.Add(new System.Windows.Input.CommandBinding());
        }

        public void CanOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetUpBindings()
        {
            Window.CommandBindings.Add(new CommandBinding(AppCommands.Open, Open, CanOpen));
        }

    }
}
