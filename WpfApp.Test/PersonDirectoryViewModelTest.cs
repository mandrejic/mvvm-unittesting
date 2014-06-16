using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp.ViewModel;
using FluentAssertions;
using System.ComponentModel;
using WpfApp.Events;
using Moq;
using WpfApp.Model;
using Microsoft.Practices.Prism.PubSubEvents;

namespace WpfApp.Test
{
	[TestClass]
	public class PersonDirectoryViewModelTest : TestBase
	{
		PersonDirectoryViewModel viewModel;
		Action<object> personDirectoryUpdateCallback;
		Action<Person> personDeletedCallback;

		[TestInitialize]
		public void TestInitialize()
		{
			TestSetup();
		}

		private void GivenEmptyViewModel()
		{
			viewModel = new PersonDirectoryViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object);
		}

		private void GivenPersonDirectory()
		{
			viewModel = new PersonDirectoryViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object);

			viewModel.RefreshAsync().Wait();
		}

		private void WhenDirectoryUpdateEventIsReceived()
		{
			personDirectoryUpdateCallback.Invoke(null);
		}

		private void WhenPersonDeletedEventIsReceived(Person person)
		{
			personDeletedCallback.Invoke(person);
		}

		[TestMethod]
		public void PersonDirectoryViewModel_WhenRefreshIsCalled_PersonDirectoryShouldBeUpdated()
		{
			//Arrange
			viewModel = new PersonDirectoryViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object);

			//Act
			viewModel.RefreshAsync().Wait();

			//Assert
			CollectionAssert.AreEqual(persons, viewModel.PersonDirectory);
		}

		[TestMethod]
		public void PersonDirectoryViewModel_GivenPersonDirectory_WhenPersonIsSelected_PropertyChangedShouldBeRaised()
		{
			GivenPersonDirectory();

			viewModel.MonitorEvents();

			viewModel.SelectedPerson = persons[0];

			viewModel.ShouldRaise("PropertyChanged")
					.WithSender(viewModel)
					.WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SelectedPerson");
		}

		[TestMethod]
		public void PersonDirectoryViewModel_GivenPersonDirectory_WhenPersonIsSelected_SelectedPersonChangeEventShouldBePublishedExactlyOnce()
		{
			GivenPersonDirectory();

			viewModel.SelectedPerson = persons[0];

			currentPersonChangeEventMock.Verify(x => x.Publish(It.IsAny<Person>()), Times.Once, "Selected person change event was published exactly one.");
		}

		[TestMethod]
		public void PersonDirectoryViewModel_GivenPersonDirectory_WhenPersonIsSelected_SelectedPersonChangeEventShouldBePublishedWithSelectedPerson()
		{
			GivenPersonDirectory();

			viewModel.SelectedPerson = persons[0];

			currentPersonChangeEventMock.Verify(x => x.Publish(It.Is<Person>(p => object.ReferenceEquals(p, persons[0]))), "Selected person change event was published, but person other then selected was sent with it.");
		}

		[TestMethod]
		public void PersonDirectoryViewModel_GivenEmptyViewModel_WhenDirectoryUpdateEventIsReceived_PersonDirectoryShouldBeUpdated()
		{
			personDirectoryUpdatedEventMock.Setup(
				x =>
				x.Subscribe(
					It.IsAny<Action<object>>(),
					It.IsAny<ThreadOption>(),
					It.IsAny<bool>(),
					It.IsAny<Predicate<object>>()))
					.Callback<Action<object>, ThreadOption, bool, Predicate<object>>(
					(e, t, b, a) => personDirectoryUpdateCallback = e);

			GivenEmptyViewModel();
			WhenDirectoryUpdateEventIsReceived();
			CollectionAssert.AreEqual(persons, viewModel.PersonDirectory);
		}

		public void PersonDirectoryViewModel_GivenPersonDirectory_WhenPersonDeletedEventIsReceived_PersonShouldBeRemovedFromDirectory()
		{
			personDeletedEventMock.Setup(
				x =>
				x.Subscribe(
				It.IsAny<Action<Person>>(),
				It.IsAny<ThreadOption>(),
				It.IsAny<bool>(),
				It.IsAny<Predicate<object>>()))
				.Callback<Action<object>, ThreadOption, bool, Predicate<object>>(
				(e, t, b, a) => personDeletedCallback = e);

			GivenPersonDirectory();
			WhenPersonDeletedEventIsReceived(persons[0]);
			CollectionAssert.DoesNotContain(viewModel.PersonDirectory, persons[0]);
		}
	}
}
