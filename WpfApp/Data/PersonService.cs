using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Data
{
	public class PersonService : IPersonService
	{
		public IEnumerable<Person> GetPersons()
		{
			return FakeDatabase.Persons;
		}

		public bool CreatePerson(Person person)
		{
			FakeDatabase.AddPerson(person);
			return true;
		}

		public bool UpdatePerson(Person person)
		{
			return FakeDatabase.UpdatePerson(person);

		}

		public bool DeletePerson(int personId)
		{
			return FakeDatabase.DeletePerson(personId);
		}
	}
}
