using Microsoft.Practices.Prism.PubSubEvents;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Data;
using WpfApp.Events;
using WpfApp.Helpers;
using WpfApp.Model;
using WpfApp.Services;

namespace WpfApp.Test
{
	public class TestBase
	{
		protected Mock<IPersonService> personServiceMock = new Mock<IPersonService>();
		protected Mock<IDispatcher> dispatcherMock = new Mock<IDispatcher>();
		protected Mock<IEventAggregator> aggregatorMock = new Mock<IEventAggregator>();
		protected Mock<IDialogService> dialogServiceMock = new Mock<IDialogService>();

		protected Mock<SelectedPersonChangeEvent> currentPersonChangeEventMock = new Mock<SelectedPersonChangeEvent>();
		protected Mock<PersonDirectoryUpdatedEvent> personDirectoryUpdatedEventMock = new Mock<PersonDirectoryUpdatedEvent>();
		protected Mock<PersonDeletedEvent> personDeletedEventMock = new Mock<PersonDeletedEvent>();

		protected readonly List<Person> persons;

		public TestBase()
		{
			persons = new List<Person>()
			{
				new Person() { Id = 1, Age = 20, FirstName = "Name 1", LastName = "Surname 1" },
				new Person() { Id = 2, Age = 30, FirstName = "Name 2", LastName = "Surname 2"},
				new Person() { Id = 3, Age = 40, FirstName = "Name 3", LastName = "Surname 3"}
			};
		}

		protected void TestSetup()
		{
			currentPersonChangeEventMock = new Mock<SelectedPersonChangeEvent>();
			personDirectoryUpdatedEventMock = new Mock<PersonDirectoryUpdatedEvent>();
			personDeletedEventMock = new Mock<PersonDeletedEvent>();

			PersonServiceSetup();
			DispatcherSetup();
			AggregatorSetup();
		}

		private void PersonServiceSetup()
		{
			personServiceMock.Setup(x => x.GetPersons()).Returns(() => persons);
		}

		private void DispatcherSetup()
		{
			dispatcherMock.Setup(x => x.Invoke(It.IsAny<Action>())).Callback((Action a) => a());
			dispatcherMock.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback((Action a) => a());
		}

		private void AggregatorSetup()
		{
			aggregatorMock.Setup(x => x.GetEvent<SelectedPersonChangeEvent>()).Returns(currentPersonChangeEventMock.Object);
			aggregatorMock.Setup(x => x.GetEvent<PersonDirectoryUpdatedEvent>()).Returns(personDirectoryUpdatedEventMock.Object);
			aggregatorMock.Setup(x => x.GetEvent<PersonDeletedEvent>()).Returns(personDeletedEventMock.Object);
		}

		protected void AssertValidMessageBoxWasDisplayed(object expectedViewModel, string expectedText, string expectedCaption, MessageBoxButton expectedButtons, MessageBoxImage expectedImage, string failMessage)
		{
			dialogServiceMock.Verify(x => x.ShowMessageBox(
				It.Is<object>(o => object.ReferenceEquals(o, expectedViewModel)),
				It.Is<string>(s => s.Equals(expectedText)),
				It.Is<string>(s => s.Equals(expectedCaption)),
				It.Is<MessageBoxButton>(b => b.Equals(expectedButtons)),
				It.Is<MessageBoxImage>(i => i.Equals(expectedImage))), Times.Once, failMessage);
		}
	}
}
