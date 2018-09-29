using System;
using System.Windows.Input;

namespace DKDG.Models.Utils
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Func<object, bool> _canExecute;

        private readonly Action<object> _execute;

        private readonly Func<bool> canExecuteEvaluator;

        private readonly Action methodToExecute;

        #endregion Fields

        #region Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion Events

        #region Constructors

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (x => true);
        }

        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null) { }

        #endregion Constructors

        #region Methods

        public bool CanExecute(object parameter)
        {
            if (canExecuteEvaluator == null)
                return true;
            else
                return canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            methodToExecute.Invoke();
        }

        #endregion Methods
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Predicate<T> _canExecute = null;
        private readonly Action<T> _execute = null;

        #endregion Fields

        #region Events

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        #endregion Events

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Creates a new command with conditional execution.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region Methods

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion Methods
    }
}
