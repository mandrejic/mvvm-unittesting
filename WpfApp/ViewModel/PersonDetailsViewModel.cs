using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Data;
using WpfApp.Events;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Services;
using WpfApp.View;

namespace WpfApp.ViewModel
{
	public class PersonDetailsViewModel : ViewModelBase
	{
		Person person;

		public ICommand NewPersonCommand { get; private set; }
		public ICommand EditPersonCommand { get; private set; }
		public ICommand DeletePersonCommand { get; private set; }

		public PersonDetailsViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService) 
			: base(personService, dispatcher, aggregator, dialogService)
		{
			aggregator.GetEvent<SelectedPersonChangeEvent>().Subscribe(OnSelectedPersonChanged, ThreadOption.BackgroundThread);

			NewPersonCommand = new DelegateCommand(NewPerson);
			EditPersonCommand = new DelegateCommand(EditPerson, CanEditPerson);
			DeletePersonCommand = new DelegateCommand(DeletePerson, CanDeletePerson);
		}

		private void OnSelectedPersonChanged(Person person)
		{
			this.Person = person;
		}

		private void OnPersonUpdated()
		{
			OnPropertyChanged("Person");
			((DelegateCommand)EditPersonCommand).RaiseCanExecuteChanged();
			((DelegateCommand)DeletePersonCommand).RaiseCanExecuteChanged();
		}

		public Person Person
		{
			get
			{
				return this.person;
			}
			set
			{
				if (this.person != value)
				{
					this.person = value;
					OnPersonUpdated();
				}
			}
		}

		private void NewPerson()
		{
			PersonDialogViewModel viewModel = new PersonDialogViewModel(personService, dispatcher, aggregator, dialogService, null);

			if (dialogService.ShowDialog<PersonDialog>(this, viewModel) == true)
			{
				aggregator.GetEvent<PersonDirectoryUpdatedEvent>().Publish(null);
			}
		}

		private void EditPerson()
		{
			PersonDialogViewModel viewModel = new PersonDialogViewModel(personService, dispatcher, aggregator, dialogService, person);

			if (dialogService.ShowDialog<PersonDialog>(this, viewModel) == true)
			{
				this.person.FirstName = viewModel.Person.FirstName;
				this.person.LastName = viewModel.Person.LastName;
				this.person.Age = viewModel.Person.Age;
			}
		}

		private bool CanEditPerson()
		{
			return IsPersonSelected;
		}

		private void DeletePerson()
		{
			if (dialogService.ShowMessageBox(this, "Are you sure?", "Confirm deletion", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
			{
				if (personService.DeletePerson(this.person.Id))
				{
					aggregator.GetEvent<PersonDeletedEvent>().Publish(person);
					dialogService.ShowMessageBox(this, "Person successfully deleted", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
				}
				else
				{
					dialogService.ShowMessageBox(this, "Error deleting person", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
				}
			}
		}

		private bool CanDeletePerson()
		{
			return IsPersonSelected;
		}

		private bool IsPersonSelected
		{
			get
			{
				return this.person != null;
			}
		}
	}
}
