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

        public void SelectTab(object sender, ExecutedRoutedEventArgs e)
        {
            Window.Model.SelectedTabIndex = (int)e.Parameter;
        }

        public void CanSelectTab(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is int && (int)e.Parameter < Window.Model.Tabs.Count;
        }

        public void CloseTab(object sender, ExecutedRoutedEventArgs e)
        {
            Window.Model.Close((int)e.Parameter);
        }

        public void CanCloseTab(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (int)e.Parameter < Window.Model.Tabs.Count;
        }

        public void OpenSettings(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO implement
            MessageBox.Show("There are no settings for now you silly!");
        }

        public void CanOpenSettings(object senter, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetUpBindings()
        {
            Window.CommandBindings.Add(new CommandBinding(AppCommands.OpenTab, Open, CanOpen));
            Window.CommandBindings.Add(new CommandBinding(AppCommands.SelectTab, SelectTab, CanSelectTab));
            Window.CommandBindings.Add(new CommandBinding(AppCommands.CloseTab, CloseTab, CanCloseTab));
            Window.CommandBindings.Add(new CommandBinding(AppCommands.OpenSettings, OpenSettings, CanOpenSettings));
        }

    }
}
