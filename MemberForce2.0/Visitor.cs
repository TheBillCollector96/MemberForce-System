using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberForce2._0
{
	public class Visitor : Person
	{
		//Attributes

		private int visitorID;
		private TimeSpan timeIN;
		private TimeSpan timeOUT;

		//Constructor

		public Visitor(string name, string surname, string nationalID, string phone, string email, TimeSpan timeIN, int visitorID)
			: base(name, surname, nationalID, phone, email)
		{
			this.timeIN = timeIN;
			this.visitorID = visitorID;
		}

		//Getters (Accessors)

		public TimeSpan getTimeIN()
		{
			return timeIN;
		}

		public TimeSpan getTimeOUT()
		{
			return timeOUT;
		}

		public int getVisitorID()
		{
			return visitorID;
		}
		
		//Setters (Mutators)
		public void setTimeIN(TimeSpan timeIN)
		{
			this.timeIN = timeIN;
		}

		public void setTimeOUT(TimeSpan timeOUT)
		{
			this.timeOUT = timeOUT;
		}

		public void setVisitorID(int visitorID)
		{
			this.visitorID = visitorID;
		}
	}
}
