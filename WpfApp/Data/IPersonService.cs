using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Data
{
	public interface IPersonService
	{
		IEnumerable<Person> GetPersons();

		bool CreatePerson(Person person);

		bool UpdatePerson(Person person);

		bool DeletePerson(int personId);
	}
}
