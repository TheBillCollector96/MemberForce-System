using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class Residence
	{

		//Attributes
		private int resID;
		private string resName;
		private string resStreet;
		private string resCity;
		private string resZip;
		private int numOfRooms;
		private int availableStudents;

		//Constructor
		public Residence(int resID, string resName, string resStreet, string resCity, string resZip, int numOfRooms, int availableStudents)
		{
			this.resID = resID;
			this.resName = resName;
			this.resStreet = resStreet;
			this.resCity = resCity;
			this.resZip = resZip;
			this.numOfRooms = numOfRooms;
			this.availableStudents = availableStudents;
		}

		//Getters (Mutators)

		public int getResID()
		{
			return resID;
		}

		public string getResName()
		{
			return resName;
		}

		public string getResStreet()
		{
			return resStreet;
		}

		public string getResCity()
		{
			return resCity;
		}

		public string getZip()
		{
			return resZip;
		}

		public int getNumOfRooms()
		{
			return numOfRooms;
		}

		public int getAvailableStu()
		{
			return availableStudents;
		}

		//Setters (Mutators)

		public void setResID(int resID)
		{
			this.resID = resID;
		}

		public void setResName(string resName)
		{
			this.resName = resName;
		}

		public void setResStreet(string street)
		{
			this.resStreet = street;
		}

		public void setResCity(string city)
		{
			this.resCity = city;
		}

		public void setResZip(string zip)
		{
			this.resZip = zip;
		}

		public void setNumOfRooms(int numOfRooms)
		{
			this.numOfRooms = numOfRooms;
		}

		public void setAvailableStudents(int students)
		{
			this.availableStudents = students;
		}
	}
}
