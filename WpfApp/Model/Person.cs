using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
	public class Person : BindableBase
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
					OnPropertyChanged("FirstName");
					OnPropertyChanged("FullName");
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
					OnPropertyChanged("LastName");
					OnPropertyChanged("FullName");
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
					OnPropertyChanged("Age");
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
