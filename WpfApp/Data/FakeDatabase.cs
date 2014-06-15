using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Data
{
	public class FakeDatabase
	{
		static int currentId = 1;

		static List<Person> persons = new List<Person>()
		{
			new Person() { FirstName = "John", LastName = "Doe", Age = 21, Id = currentId++ },
			new Person() { FirstName = "Jane", LastName = "Doe", Age = 23, Id = currentId++ },
			new Person() { FirstName = "Marko", LastName = "Markovic", Age = 30, Id = currentId++ }
		};

		public static List<Person> Persons
		{
			get
			{
				return new List<Person>(persons);
			}
		}

		public static void AddPerson(Person person)
		{
			person.Id = currentId++;
			persons.Add(person);
		}

		public static bool UpdatePerson(Person person)
		{
			int index = persons.FindIndex(p => p.Id == person.Id);
			if (index < 0)
			{
				return false;
			}
			persons[index] = person;
			return true;
		}

		public static bool DeletePerson(int personId)
		{
			int index = persons.FindIndex(p => p.Id == personId);
			if (index < 0)
			{
				return false;
			}
			persons.RemoveAt(index);
			return true;
		}
	}
}
