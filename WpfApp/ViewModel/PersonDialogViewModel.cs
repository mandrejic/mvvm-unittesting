using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Data;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Services;

namespace WpfApp.ViewModel
{
	public class PersonDialogViewModel : ViewModelBase
	{
		Person person;
		bool? dialogResult;
		
		public PersonDialogViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService, Person person)
			: base(personService, dispatcher, aggregator, dialogService)
		{
			this.person = new Person();
			if (person != null)
			{
				this.person.Id = person.Id;
				this.person.FirstName = person.FirstName;
				this.person.LastName = person.LastName;
				this.person.Age = person.Age;
				this.Title = "Edit person";
				this.CommandName = "Update";
				ActionCommand = new DelegateCommand(Edit);
			}
			else
			{
				this.Title = "New person";
				this.CommandName = "Create";
				ActionCommand = new DelegateCommand(Add);
			}
		}

		public string Title { get; protected set; }
		public string CommandName { get; protected set; }
		public DelegateCommand ActionCommand { get; private set; }

		public bool? DialogResult
		{
			get { return this.dialogResult; }
			set
			{
				if (this.dialogResult != value)
				{
					this.dialogResult = value;
					this.OnPropertyChanged("DialogResult");
				}
			}
		}

		public Person Person
		{
			get
			{
				return this.person;
			}
		}

		private void Edit()
		{
			if (personService.UpdatePerson(person))
			{
				DialogResult = true;
			}
		}

		private void Add()
		{
			if (personService.CreatePerson(person))
			{
				DialogResult = true;
			}
		}
	}
}
