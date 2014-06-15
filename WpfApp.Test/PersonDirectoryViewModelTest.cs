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
	}
}
