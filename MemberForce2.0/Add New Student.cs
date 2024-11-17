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
    public partial class Add_New_Student : Form
    {
		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");
		MySqlDataAdapter dataAdapter;

        private StudentResidenceVisitor frmResVisitor;
		private Residence residence;
		private int stID;
		private int resIndex;

        public Add_New_Student(StudentResidenceVisitor frmResVisitor, int stID, Residence residence, int index)
        {
            InitializeComponent();
            this.frmResVisitor = frmResVisitor;
            this.stID = stID;
			this.resIndex = index;
			this.residence = residence;
			txtAddRes.Text = residence.getResName();
			fillStudentFields();
        }

        public void fillStudentFields()
		{
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT * FROM student WHERE stID = " + stID;
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						txtNationalID.Text = dataReader.GetValue(1).ToString();
						txtAddName.Text = dataReader.GetValue(2).ToString();
						txtAddSurname.Text = dataReader.GetValue(3).ToString();
						if (dataReader.GetValue(4).ToString()[0] == 'M')
							txtAddGender.Text = "Male";
						else
							txtAddGender.Text = "Female";
						txtAddDOB.Text = dataReader.GetValue(5).ToString();
						txtAddCourse.Text = dataReader.GetValue(6).ToString();
						txtxAddAcademicYear.Text = dataReader.GetValue(7).ToString();
						txtAddPhone.Text = dataReader.GetValue(8).ToString();
						txtAddEmail.Text = dataReader.GetValue(9).ToString();

						break;
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
		}

		public bool validateUsername(string username)
		{
			bool found= false;

			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT username FROM residencestudent";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						if (dataReader.GetValue(0).ToString() == username)
						{
							found = true;
							break;
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

			return found;
		}

		private bool incrementAvailableStudent()
		{
			bool updated = false;

			//Increment from ArrayList
			int availableStudents = residence.getAvailableStu();
			availableStudents++; //Increment available students
			residence.setAvailableStudents(availableStudents);

			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					//Updates the timeOUT in the database
					String query = "UPDATE residence SET availableStudents = " + residence.getAvailableStu() + " WHERE resID = " + residence.getResID();
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter();
					dataAdapter.UpdateCommand = command;
					if (dataAdapter.UpdateCommand.ExecuteNonQuery() == 1)
					{
						updated = true;
					}
					else
					{
						MessageBox.Show("ERROR: Failed to update database. Please call administrator.");
					}
					command.Dispose();
				}
				else
				{
					MessageBox.Show("Connection Failed.");
				}
				con.Close();
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}

			return updated;
		}

		private void btnAddStudent_Click(object sender, EventArgs e)
		{
			if(txtAddRoom.Text != "")
			{
				if (txtAddUsername.Text != "")
				{
					if (!validateUsername(txtAddUsername.Text))
					{
						if (txtAddPassword.Text != "")
						{
							if (txtConfirmPassword.Text != "")
							{
								if (txtAddPassword.Text == txtConfirmPassword.Text)
								{
									try
									{
										con.Open();
										if (con.State == ConnectionState.Open)
										{

											//Store the records into the database
											String query = $"INSERT INTO residencestudent(stID, resID, roomNumber, monthlyVisits, visitorRestricted, password, username) VALUES ('" + stID + "','" + residence.getResID() + "','" + txtAddRoom.Text + "','" + 0 + "','" + 0 + "','" + txtConfirmPassword.Text + "','" + txtAddUsername.Text + "')";
											MySqlCommand command = new MySqlCommand(query, con);
											dataAdapter = new MySqlDataAdapter();
											dataAdapter.InsertCommand = command;
											if (dataAdapter.InsertCommand.ExecuteNonQuery() == 1)
											{
												if (incrementAvailableStudent())
												{
													frmResVisitor.updateResidence(resIndex, residence);
													frmResVisitor.refreshResStudents(resIndex);
													MessageBox.Show("New Student Added Successfully.");
													this.Close();
												}
												else
												{
													MessageBox.Show("ERROR: Failed to increment AvailableStudents in DB. Please Call Administrator");
												}
												
											}
											else
											{
												MessageBox.Show("Failed to add new Student. \nCall Administrator");
											}
										}
										else
										{
											MessageBox.Show("Connection Failed.");
										}
										con.Close();
									}
									catch (SqlException ex)
									{
										MessageBox.Show(ex.Message);
									}
								}
								else
								{
									MessageBox.Show("Entered passwords don't match.");
								}
							}
							else
							{
								MessageBox.Show("Please Confirm your entered password");
							}
						}
						else
						{
							MessageBox.Show("Please enter a password.");
						}
					}
					else
					{
						MessageBox.Show("Username already exist. Try another one.");
					}
				}
				else
				{
					MessageBox.Show("Please enter a unique username for the student.");
				}
			}
			else
			{
				MessageBox.Show("Please specify the room number for the new student");
			} 
			
		}
	}
}
