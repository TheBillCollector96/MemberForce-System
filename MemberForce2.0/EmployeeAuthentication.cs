using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemberForce2._0
{
	public partial class EmployeeAuthentication : Form
	{
		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com; User Id=root; Password=password; Database=memberforce");
		MySqlDataAdapter dataAdapter;
		DataSet dataSet;
		private MemberForce frmMain;
		private String frmOpen;


		public EmployeeAuthentication(MemberForce frmMain, String frmOpen)
		{
			InitializeComponent();
			this.frmMain = frmMain;
			this.frmOpen = frmOpen;
		}

		private string authenticateEmployee(string username, string password)
		{
			string occupation = "none";

			//Authenticate user credentials in the database
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT empUsername, empPassword, empOccupation FROM employee";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					bool authenticated = false;
					while (dataReader.Read())
					{
						if (dataReader.GetValue(0).ToString() == username && dataReader.GetValue(1).ToString() == password)
						{
							occupation = dataReader.GetValue(2).ToString();
						}
					}

				}
				else
				{
					MessageBox.Show("Connection Failed");
				}
				con.Close();
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}

			return occupation;
		}

		private int getTheEmpID(String username)
		{
			int empID = -1;

			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT empID FROM employee WHERE empUsername = '" + username + "'";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						empID = (int)dataReader.GetValue(0);
					}

				}
				else
				{
					MessageBox.Show("Connection Failed");
				}
				con.Close();
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}

			return empID;
		}

		//Validate Authority based on "occupation" and "frmOpen"
		//Get empID
		//Open Desired form with the empID
		private void btnLogIn_Click(object sender, EventArgs e)
		{
			//Authenticate employee
			int empID = -1;
			string occupation = authenticateEmployee(txtUsername.Text, txtPassword.Text);
			if (occupation != "none")
			{
				//Validate Authority based on "occupation" and "frmOpen"
				if (frmOpen == "ResVisitor")
				{
					if (occupation == "Security" || occupation == "House Committee")
					{
						empID = getTheEmpID(txtUsername.Text);
						if (empID != -1)
						{
							frmMain.openResidenceVisitor(empID);
							this.Close();
						}
						else
						{
							MessageBox.Show("ERROR: empID not found. Please call administrator.");
						}
					}
					else
					{
						MessageBox.Show("Access Denied. Unfortunately you don't have authorised access.");
					}
				}
				else if (frmOpen == "PermitGen")
				{
					if (occupation == "Security" || occupation == "NWU Admin")
					{
						empID = getTheEmpID(txtUsername.Text);
						if (empID != -1)
						{
							frmMain.openPermitGenerator(empID);
							this.Close();
						}
						else
						{
							MessageBox.Show("ERROR: empID not found. Please call administrator.");
						}
					}
					else
					{
						MessageBox.Show("Access Denied. Unfortunately you don't have authorised access.");
					}
				} 
				else if (frmOpen == "ReportMisc")
				{
					if (occupation == "Security" || occupation == "Disciplinary Committee")
					{
						empID = getTheEmpID(txtUsername.Text);
						if (empID != -1)
						{
							frmMain.openMisconductingStudent(empID);
							this.Close();
						}
						else
						{
							MessageBox.Show("ERROR: empID not found. Please call administrator.");
						}
					}
					else
					{
						MessageBox.Show("Access Denied. Unfortunately you don't have authorised access.");
					}
				}
				else
				{
					MessageBox.Show("ERROR: Open Form not found. Please call administrator.");
				}
			}
			else
			{
				MessageBox.Show("Incorrect StaffID & Password! \nTry Again");
				txtPassword.Text = "";
				txtUsername.Text = "";
			}
		}
	}
}
