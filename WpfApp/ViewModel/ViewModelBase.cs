using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Data;
using WpfApp.Helpers;
using WpfApp.Services;

namespace WpfApp.ViewModel
{
	public abstract class ViewModelBase : NotificationObject
	{
		protected bool isBusy;
		protected IPersonService personService;
		protected IDispatcher dispatcher;
		protected IEventAggregator aggregator;
		protected IDialogService dialogService;

		public ViewModelBase(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
		{
			if (personService == null)
			{
				throw new ArgumentNullException("personService");
			}

			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}

			if (aggregator == null)
			{
				throw new ArgumentNullException("aggregator");
			}

			if (dialogService == null)
			{
				throw new ArgumentNullException("dialogService");
			}

			this.personService = personService;
			this.dispatcher = dispatcher;
			this.aggregator = aggregator;
			this.dialogService = dialogService;

			isBusy = false;
		}

		public bool IsBusy
		{
			get
			{
				return isBusy;
			}
			protected set
			{
				isBusy = value;
				this.RaisePropertyChanged("IsBusy");

			}
		}

	}
}
