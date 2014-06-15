using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp.Helpers
{
	public class DispatcherWrapper : IDispatcher
	{
		static object syncLock = new object();
		static DispatcherWrapper instance;
		Dispatcher dispatcher;

		public static IDispatcher Instance
		{
			get
			{
				lock (syncLock)
				{
					if (instance == null)
					{
						instance = new DispatcherWrapper();
					}
				}
				return instance;
			}
		}

		public DispatcherWrapper()
		{
			if (Application.Current != null)
			{
				dispatcher = Application.Current.Dispatcher;
			}
			else
			{
				//this is useful for unit tests where there is no application running 
				dispatcher = Dispatcher.CurrentDispatcher;
			}
		}
		public void Invoke(Action action)
		{
			dispatcher.Invoke(action);
		}

		public void BeginInvoke(Action action)
		{
			dispatcher.BeginInvoke(action);
		}
	}
}
