using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	class Employee : Person
	{
		//Attributes

		private int empID;
		private string occupation;
		//private string username;

		//Constructors

		public Employee(string nationalID, string name, string surname, char gender, DateTime dateOfBirth, string phone, string email, int empID, string occupation)
			:base(nationalID, name, surname, gender, dateOfBirth, phone, email)
		{
			this.empID = empID;
			this.occupation = occupation;
		}

		//Getters (Accessors)

		public int getEmpID()
		{
			return empID;
		}

		public string getOccupation()
		{
			return occupation;
		}

		//Setters (Mutators)
		public void setEmpID(int empID)
		{
			this.empID = empID;
		}

		public void setOccupation(string occupation)
		{
			this.occupation = occupation;
		}
	}
}
