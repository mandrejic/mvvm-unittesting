using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp.ViewModel;
using FluentAssertions;
using System.ComponentModel;
using WpfApp.View;
using Moq;
using System.Windows;
using WpfApp.Model;

namespace WpfApp.Test
{
	[TestClass]
	public class PersonDetailsViewModelTest : TestBase
	{
		PersonDetailsViewModel viewModel;

		[TestInitialize]
		public void TestInitialize()
		{
			TestSetup();
		}

		private void GivenEmptyViewModel()
		{
			viewModel = new PersonDetailsViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object);
		}

		private void GivenPerson()
		{
			viewModel = new PersonDetailsViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object);

			viewModel.Person = persons[0];
		}

		private void WhenDeleteConfirmationIsReceived()
		{
			dialogServiceMock.Setup(x => x.ShowMessageBox(
				It.IsAny<object>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<MessageBoxButton>(),
				It.IsAny<MessageBoxImage>())).Returns(MessageBoxResult.Yes);
		}

		private void WhenDeletionIsSuccessful()
		{
			WhenDeleteConfirmationIsReceived();
			personServiceMock.Setup(x => x.DeletePerson(It.IsAny<int>())).Returns(true);
			viewModel.DeletePersonCommand.Execute(null);
		}

		private void WhenDeletionIsUnsuccessful()
		{
			WhenDeleteConfirmationIsReceived();
			personServiceMock.Setup(x => x.DeletePerson(It.IsAny<int>())).Returns(false);
			viewModel.DeletePersonCommand.Execute(null);
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenEmptyViewModel_WhenPersonIsSet_ThenPropertyChangedShouldBeRaised()
		{
			GivenEmptyViewModel();

			viewModel.MonitorEvents();

			viewModel.Person = persons[0];

			viewModel.ShouldRaise("PropertyChanged")
					.WithSender(viewModel)
					.WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "Person");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenEmptyViewModel_WhenNewPersonCommandIsExecuted_ThenPersonDialogShouldBeDisplayed()
		{
			GivenEmptyViewModel();

			viewModel.NewPersonCommand.Execute(null);

			dialogServiceMock.Verify(x => x.ShowDialog<PersonDialog>(It.Is<object>(o => object.ReferenceEquals(o, viewModel)), It.IsAny<object>()), Times.Once, "New person dialog was not displayed.");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenEmptyViewModel_WhenNewPersonWasCreatedSuccessfully_ThenDirectoryUpdatedEventShouldBePublished()
		{
			GivenEmptyViewModel();

			dialogServiceMock.Setup(x => x.ShowDialog<PersonDialog>(It.IsAny<object>(), It.IsAny<object>())).Returns(true);

			viewModel.NewPersonCommand.Execute(null);

			personDirectoryUpdatedEventMock.Verify(x => x.Publish(It.IsAny<object>()), Times.Once, "Person directory updated event was not published");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenEmptyViewModel_WhenNewPersonWasNotCreatedSuccessfully_ThenDirectoryUpdatedEventShouldNotBePublished()
		{
			GivenEmptyViewModel();

			dialogServiceMock.Setup(x => x.ShowDialog<PersonDialog>(It.IsAny<object>(), It.IsAny<object>())).Returns(false);

			viewModel.NewPersonCommand.Execute(null);

			personDirectoryUpdatedEventMock.Verify(x => x.Publish(It.IsAny<object>()), Times.Never, "Person directory updated event was published, but it shouldn't have been");
		}


		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeleteCommandIsExecuted_ThenUserShouldBePromptedToConfirm()
		{
			GivenPerson();

			viewModel.DeletePersonCommand.Execute(null);

			AssertValidMessageBoxWasDisplayed(
				viewModel, 
				"Are you sure?", 
				"Confirm deletion",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question, 
				"User should have been presented (only once) with a message box to confirm deletion");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeleteConfirmationIsReceived_ThenDeleteShouldBeCalledOnService()
		{
			GivenPerson();
			WhenDeleteConfirmationIsReceived();

			viewModel.DeletePersonCommand.Execute(null);

			personServiceMock.Verify(x => x.DeletePerson(It.Is<int>(i => i == viewModel.Person.Id)), Times.Once, "A delete person service method was expected to be called, but wasn't");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeletionIsSuccessful_ThenSuccessMessageShouldBeDisplayed()
		{
			GivenPerson();

			WhenDeletionIsSuccessful();

			AssertValidMessageBoxWasDisplayed(
				viewModel,
				"Person successfully deleted",
				"Success",
				MessageBoxButton.OK,
				MessageBoxImage.Information,
				"User should have been presented (only once) with a success message");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeletionIsSuccessful_ThenPersonDeleteEventShouldBePublished()
		{
			GivenPerson();

			WhenDeletionIsSuccessful();

			personDeletedEventMock.Verify(x => x.Publish(It.Is<Person>(p => object.ReferenceEquals(p, viewModel.Person))), Times.Once, "Person deleted event was not published exactly once, or was published with wrong person as argument");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeletionIsUnsuccessful_ThenErrorMessageShouldBeDisplayed()
		{
			GivenPerson();

			WhenDeletionIsUnsuccessful();

			AssertValidMessageBoxWasDisplayed(
				viewModel,
				"Error deleting person",
				"Error",
				MessageBoxButton.OK,
				MessageBoxImage.Error,
				"User should have been presented (only once) with a error message");
		}

		[TestMethod]
		public void PersonDetailsViewModel_GivenPerson_WhenDeletionIsUnsuccessful_ThenNoPersonDeleteEventShouldBePublished()
		{
			GivenPerson();

			WhenDeletionIsUnsuccessful();

			personDeletedEventMock.Verify(x => x.Publish(It.IsAny<Person>()), Times.Never, "Expected person deleted event not to be published, but actually was");
		}
	}
}
