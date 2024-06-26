﻿using System;
using System.Windows.Input;

namespace Labyrinth_WPF.ViewModel
{

    public class DelegateCommand : ICommand
    {
        private readonly Action<Object?> _execute;
        private readonly Func<Object?, Boolean>? _canExecute;

        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }

        public DelegateCommand(Func<Object?, Boolean>? canExecute, Action<Object?> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public Boolean CanExecute(Object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(Object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
