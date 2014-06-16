using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Data;
using WpfApp.Helpers;
using WpfApp.Services;

namespace WpfApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomainUnhandledException);

				IUnityContainer container = new UnityContainer();
				ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

				container.RegisterInstance<IDispatcher>(new DispatcherWrapper());
				container.RegisterType<IEventAggregator, EventAggregator>();
				container.RegisterType<IPersonService, PersonService>();
				container.RegisterType<IWindowViewModelMappings, WindowViewModelMappings>();
				container.RegisterInstance<IDialogService>(new DialogService());

				var window = container.Resolve<MainWindow>();
				window.Show();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			HandleException(e.ExceptionObject as Exception);
		}

		private static void HandleException(Exception ex)
		{
			if (ex == null)
				return;

			MessageBox.Show(
				ex.Message,
				"Location Management",
				MessageBoxButton.OK,
				MessageBoxImage.Error
				);

			Environment.Exit(1);
		}
	}
}
