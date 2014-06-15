using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Events
{
	public class PersonDeletedEvent : CompositePresentationEvent<Person>
	{
	}
}
