using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
	public class Person : NotificationObject
	{
		int id;
		public int Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		string firstName;
		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				if (firstName != value)
				{
					firstName = value;
					RaisePropertyChanged("FirstName");
					RaisePropertyChanged("FullName");
				}
			}
		}

		string lastName;
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				if (lastName != value)
				{
					lastName = value;
					RaisePropertyChanged("LastName");
					RaisePropertyChanged("FullName");
				}
			}
		}

		int age;
		public int Age
		{
			get
			{
				return age;
			}
			set
			{
				if (age != value)
				{
					age = value;
					RaisePropertyChanged("Age");
				}
			}
		}

		public string FullName
		{
			get
			{
				return string.Format("{0} {1}", firstName ?? string.Empty, lastName ?? string.Empty);
			}
		}

	}
}
