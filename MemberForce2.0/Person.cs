using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class Person
	{
		//Attributes

		private string nationalID;
		private string name;
		private string surname;
		private char gender;
		private DateTime dateOfBirth;
		private string phone;
		private string email;

		//Constructors

		public Person(string nationalID, string name, string surname, char gender, DateTime dateOfBirth, string phone, string email)
		{
			this.nationalID = nationalID;
			this.name = name;
			this.surname = surname;
			this.gender = gender;
			this.dateOfBirth = dateOfBirth;
			this.phone = phone;
			this.email = email;
		}

		//Constructor (For Visitor)
		public Person(string name, string surname, string nationalID, string phone, string email)
		{
			this.name = name;
			this.surname = surname;
			this.nationalID = nationalID;
			this.phone = phone;
			this.email = email;
			this.gender = '!';
			this.dateOfBirth = new DateTime(0001,01,01);
		}

		//Getters (Accessors)

		public string getNationalID()
		{
			return nationalID;
		}

		public string getName()
		{ 
			return name;
		}

		public string getSurname() 
		{ 
			return surname; 
		}

		public char getGender() 
		{ 
			return gender; 
		}

		public DateTime getDateOfBirth ()
		{ 
			return dateOfBirth; 
		}

		public string getPhone() 
		{ 
			return phone; 
		}

		public string getEmail() 
		{ 
			return email; 
		}

		//Setters (Mutators)
		public void setNationalID(string nationalID)
		{
			this.nationalID = nationalID;
		}

		public void setName(string name)
		{
			this.name = name;
		}

		public void setSurname(string surname)
		{
			this.surname = surname;
		}

		public void setGender(char gender)
		{
			this.gender = gender;
		}

		public void setDateOfBirth(DateTime date)
		{
			this.dateOfBirth = date;
		}

		public void setPhone(string phone)
        {
			this.phone = phone;
        }

		public void setEmail(string email)
        {
			this.email = email;
        }
	}
}
