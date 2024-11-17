using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class Student : Person
	{
		//Attributes

		private int stID;
		private string universityID;
		private string course;
		private int academicYear;
		private byte[] arrImage;

		//Constructors

		public Student(string nationalID, string name, string surname, char gender, DateTime dateOfBirth, string phone, string email, int stID, string course, int academicYear, string universityID)
			: base(nationalID, name, surname, gender, dateOfBirth, phone, email)
		{
			this.stID = stID;
			this.course = course;
			this.academicYear = academicYear;
			this.universityID = universityID;
		}

		public Student(string nationalID, string name, string surname, char gender, DateTime dateOfBirth, string phone, string email, int stID, string course, int academicYear, string universityID, byte[] arrImage)
			: base(nationalID, name, surname, gender, dateOfBirth, phone, email)
		{
			//Constructor will be used by Permit Generator
			this.stID = stID;
			this.course = course;
			this.academicYear = academicYear;
			this.universityID = universityID;
			this.arrImage = arrImage;
		}

		//Getters

		public int getStID()
		{
			return stID;
		}

		public string getCourse()
		{
			return course;
		}

		public int getAcademicYear()
		{
			return academicYear;
		}

		public string getUniversityID()
		{
			return universityID;
		}

		public byte[] getArrImage()
		{
			return arrImage;
		}

		//Setters
		public void setStID(int stID)
		{
			this.stID = stID;
		}

		public void setCourse(String course)
		{
			this.course = course;
		}

		public void setAcademicYear(int year)
		{
			this.academicYear = year;
		}

		public void setUniversityID(string universityID)
		{
			this.universityID = universityID;
		}

		public void setArrImage(byte[] arrImage)
		{
			this.arrImage = arrImage;
		}
	}
}
