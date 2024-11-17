using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class ResidenceStudent : Student
	{
		//Attributes
		private int resID;
		private string roomNo;
		private int monthlyVisits;
		private bool visitorRestricted;

		//Constructor

		public ResidenceStudent(string nationalID, string name, string surname, char gender, DateTime dateOfBirth, string phone, string email, int stID, string course, int academicYear, string universityID, string roomNo, int monthlyVisits, bool visitorRestricted, int resID)
			: base(nationalID, name, surname, gender, dateOfBirth, phone, email, stID, course, academicYear, universityID)
		{
			this.roomNo = roomNo;
			this.monthlyVisits = monthlyVisits;
			this.visitorRestricted = visitorRestricted;
			this.resID = resID;
		}

		//Getters (Accessors)

		public string getRoomNo()
		{
			return roomNo;
		}

		public int getMonthlyVisits()
		{
			return monthlyVisits;
		}

		public bool getVisitorRestriction()
		{
			return visitorRestricted;
		}

		public int getResID()
		{
			return resID;
		}

		//Setters (Mutators)
		public void setRoomNo(string roomNo)
		{
			this.roomNo = roomNo;
		}

		public void setMonthlyVisits(int visits)
		{
			this.monthlyVisits = visits;
		}

		public void setVisitorRestriction(bool visitorRestricted)
		{
			this.visitorRestricted = visitorRestricted;
		}

		public void setResID(int resID)
		{
			this.resID = resID;
		}
	}
}
