using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neurona.Modelos.ViewModels
{
    public sealed class ComandoAsincrono : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<object, Task> _executeWithParams;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public ComandoAsincrono(Func<Task> execute) : this(execute, () => true)
        {
        }

        public ComandoAsincrono(Func<object, Task> execute) : this(execute, () => true)
        {
        }

        public ComandoAsincrono(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public ComandoAsincrono(Func<object, Task> execute, Func<bool> canExecute)
        {
            _executeWithParams = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !(_isExecuting && _canExecute());
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                if (parameter != null && _executeWithParams != null)
                {
                    await _executeWithParams(parameter);
                }
                else
                {
                    await _execute();
                }
            }
            finally
            {
                _isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
