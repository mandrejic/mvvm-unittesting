using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.Commands
{
	public sealed class AsyncCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public Func<Task> ExecutedHandler { get; private set; }

		public Func<bool> CanExecuteHandler { get; private set; }

		public AsyncCommand(Func<Task> executedHandler, Func<bool> canExecuteHandler = null)
		{
			if (executedHandler == null)
			{
				throw new ArgumentNullException("executedHandler");
			}

			this.ExecutedHandler = executedHandler;
			this.CanExecuteHandler = canExecuteHandler;
		}

		public Task ExecuteAsync()
		{
			return this.ExecutedHandler();
		}

		public bool CanExecute()
		{
			return this.CanExecuteHandler == null || this.CanExecuteHandler();
		}

		public void RaiseCanExecuteChanged()
		{
			if (this.CanExecuteChanged != null)
			{
				this.CanExecuteChanged(this, new EventArgs());
			}
		}

		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute();
		}

		async void ICommand.Execute(object parameter)
		{
			await this.ExecuteAsync();
		}
	}
}
