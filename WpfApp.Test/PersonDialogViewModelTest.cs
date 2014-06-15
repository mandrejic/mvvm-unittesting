using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using WpfApp.ViewModel;

namespace WpfApp.Test
{
	[TestClass]
	public class PersonDialogViewModelTest : TestBase
	{
		PersonDialogViewModel viewModel;

		[TestInitialize]
		public void TestInitialize()
		{
			TestSetup();
		}

		private void GivenNoPerson()
		{
			viewModel = new PersonDialogViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object, null);
		}

		private void GivenPerson()
		{
			viewModel = new PersonDialogViewModel(personServiceMock.Object, dispatcherMock.Object, aggregatorMock.Object, dialogServiceMock.Object, persons[0]);
		}

		private void WhenCreationIsSuccessful()
		{
			personServiceMock.Setup(x => x.CreatePerson(It.IsAny<Person>())).Returns(true);
			viewModel.ActionCommand.Execute().Wait();
		}

		private void WhenCreationIsUnsuccessful()
		{
			personServiceMock.Setup(x => x.CreatePerson(It.IsAny<Person>())).Returns(false);
			viewModel.ActionCommand.Execute().Wait();
		}
		
		private void WhenUpdateIsSuccessful()
		{
			personServiceMock.Setup(x => x.UpdatePerson(It.IsAny<Person>())).Returns(true);
			viewModel.ActionCommand.Execute().Wait();
		}

		private void WhenUpdateIsUnsuccessful()
		{
			personServiceMock.Setup(x => x.UpdatePerson(It.IsAny<Person>())).Returns(false);
			viewModel.ActionCommand.Execute().Wait();
		}

		private void WhenActionCommandIsExecuted()
		{
			viewModel.ActionCommand.Execute().Wait();
		}

		[TestMethod]
		public void GivenNoPerson_ThenCreateModeShouldBeActive()
		{
			GivenNoPerson();

			Assert.AreEqual("New person", viewModel.Title);
			Assert.AreEqual("Create", viewModel.CommandName);
		}

		[TestMethod]
		public void GivenPerson_ThenEditModeShouldBeActive()
		{
			GivenPerson();

			Assert.AreEqual("Edit person", viewModel.Title);
			Assert.AreEqual("Update", viewModel.CommandName);
		}

		[TestMethod]
		public void GivenPerson_ThenPersonShouldBeCopied()
		{
			GivenPerson();
			Assert.AreNotSame(persons[0], viewModel.Person);
			Assert.AreEqual(persons[0].Id, viewModel.Person.Id);
			Assert.AreEqual(persons[0].FirstName, viewModel.Person.FirstName);
			Assert.AreEqual(persons[0].LastName, viewModel.Person.LastName);
			Assert.AreEqual(persons[0].Age, viewModel.Person.Age);
		}

		[TestMethod]
		public void GivenNoPerson_WhenActionCommandIsExecuted_ThenCreateShouldBeCalledOnService()
		{
			GivenNoPerson();
			WhenActionCommandIsExecuted();

			personServiceMock.Verify(x => x.CreatePerson(It.Is<Person>(p => object.ReferenceEquals(p, viewModel.Person))), Times.Once, "Expected CreatePerson service method to be called exactly once, but wasn't");
		}

		[TestMethod]
		public void GivenNoPerson_WhenCreationIsSuccessful_ThenDialogResultShouldBeTrue()
		{
			GivenNoPerson();
			WhenCreationIsSuccessful();

			Assert.IsTrue(viewModel.DialogResult.HasValue);
			Assert.IsTrue(viewModel.DialogResult.Value);
		}

		[TestMethod]
		public void GivenNoPerson_WhenCreationIsUnsuccessful_ThenDialogResultShouldNotBeSet()
		{
			GivenNoPerson();
			WhenCreationIsUnsuccessful();

			Assert.IsFalse(viewModel.DialogResult.HasValue);
		}

		[TestMethod]
		public void GivenPerson_WhenActionCommandIsExecuted_ThenUpdateShouldBeCalledOnService()
		{
			GivenPerson();
			WhenActionCommandIsExecuted();

			personServiceMock.Verify(x => x.UpdatePerson(It.Is<Person>(p => object.ReferenceEquals(p, viewModel.Person))), Times.Once, "Expected UpdatePerson service method to be called exactly once, but wasn't");
		}

		[TestMethod]
		public void GivenPerson_WhenUpdateIsSuccessful_ThenDialogResultShouldBeTrue()
		{
			GivenPerson();
			WhenUpdateIsSuccessful();

			Assert.IsTrue(viewModel.DialogResult.HasValue);
			Assert.IsTrue(viewModel.DialogResult.Value);
		}

		[TestMethod]
		public void GivenNoPerson_WhenUpdateIsUnsuccessful_ThenDialogResultShouldNotBeSet()
		{
			GivenPerson();
			WhenUpdateIsUnsuccessful();

			Assert.IsFalse(viewModel.DialogResult.HasValue);
		}

	}
}
