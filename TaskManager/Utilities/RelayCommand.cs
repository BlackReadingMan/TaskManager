﻿using System.Windows.Input;

namespace TaskManager.Utilities
{
	public class RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
		: ICommand
	{
		public event EventHandler? CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute(object parameter)
		{
			return canExecute == null || canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			execute(parameter);
		}
	}
}