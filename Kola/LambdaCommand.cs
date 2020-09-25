using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kola
{
    class LambdaCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event Action<object> Function;

        public LambdaCommand(Action<object> f)
        {
            Function = f;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Function?.Invoke(parameter);
        }
    }
}
