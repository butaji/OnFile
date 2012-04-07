using System;
using System.Windows.Input;

namespace OnFile.Desktop.ViewModels
{
    public class ViewCommand : ICommand
    {
        private readonly Action _action;
        private Func<bool> _canExecuteMethod;

        public ViewCommand(Action action)
        {
            _action = action;
        }

        public ViewCommand(Func<bool> canExecuteMethod, Action action)
            : this(action)
        {
            CanExecuteMethod = canExecuteMethod;
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
                throw new ArgumentException();

            _action();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteMethod == null || CanExecuteMethod();
        }

        public Func<bool> CanExecuteMethod
        {
            get
            {
                return _canExecuteMethod;
            }
            set
            {
                _canExecuteMethod = value;
                OnCanExecuteChanged();
            }
        }


        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }
    }
}