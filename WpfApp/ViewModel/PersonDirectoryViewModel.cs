using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp.Data;
using WpfApp.Events;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Services;

namespace WpfApp.ViewModel
{
	public class PersonDirectoryViewModel : ViewModelBase
	{
		readonly RangeEnabledObservableCollection<Person> personDirectory;
		Person selectedPerson;

		public PersonDirectoryViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService) 
			: base(personService, dispatcher, aggregator, dialogService)
		{
			personDirectory = new RangeEnabledObservableCollection<Person>();
			aggregator.GetEvent<PersonDirectoryUpdatedEvent>().Subscribe(OnPersonDirectoryUpdated, ThreadOption.BackgroundThread);
			aggregator.GetEvent<PersonDeletedEvent>().Subscribe(OnPersonDeleted, ThreadOption.UIThread);
		}

		public RangeEnabledObservableCollection<Person> PersonDirectory
		{
			get
			{
				return this.personDirectory;
			}
		}

		public Person SelectedPerson
		{
			get
			{
				return this.selectedPerson;
			}
			set
			{
				if (this.selectedPerson != value)
				{
					this.selectedPerson = value;
					OnPropertyChanged("SelectedPerson");
					aggregator.GetEvent<SelectedPersonChangeEvent>().Publish(this.selectedPerson);
				}
			}
		}

		public async Task RefreshAsync()
		{
			await Task.Run(() =>
			{
				IsBusy = true;
				try
				{
					var persons = this.personService.GetPersons();
					var previouslySelectedPerson = selectedPerson;
					dispatcher.Invoke(() =>
						{
							personDirectory.Clear();
							personDirectory.AddRange(persons);
							if (previouslySelectedPerson != null)
							{
								if (personDirectory.Any(p => p.Id == previouslySelectedPerson.Id))
								{
									SelectedPerson = previouslySelectedPerson;
								}
							}
						});
				}
				catch (Exception)
				{
					//TODO: Publish operation failed
				}
				finally
				{
					IsBusy = false;
				}
			});
		}

		private async void OnPersonDirectoryUpdated(object state)
		{
			await RefreshAsync();
		}

		private void OnPersonDeleted(Person person)
		{
			this.personDirectory.Remove(person);
		}
	}
}
