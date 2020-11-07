using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client
{
    public class RelayCommand : ICommand
    {

        #region Private members

        /// <summary>
        /// Action to execute.
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// Indicates can command be executed or not.
        /// </summary>
        private Func<object, bool> canExecute;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes new instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Function returns can command be executed or not.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand implementation

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }



        ///<inheritdoc cref="ICommand.CanExecute(object)"/>
        public bool CanExecute(object parameter)
        {
            if (canExecute is null)
                return true;
            return canExecute(parameter);
        }

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}
