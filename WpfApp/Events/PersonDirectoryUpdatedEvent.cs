using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Events
{
	public class PersonDirectoryUpdatedEvent : CompositePresentationEvent<object>
	{
	}
}
