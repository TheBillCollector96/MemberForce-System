using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemberForce2._0
{
	public partial class ReportMisconductingStudent : Form
	{
		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");
		MySqlDataAdapter dataAdapter;
		DataSet dataSet;
		DataTable dataTable;

		private static Employee emp; //Used to keep track of current signed in employee
		private static string reportContent;
		private MemberForce frmMain;

		public ReportMisconductingStudent(MemberForce frmMain, int empID)
		{
			InitializeComponent();
			//Initialise class contents
			this.frmMain = frmMain;
			createEmpObject(empID); 

			if (emp != null)
			{
				if (emp.getOccupation() == "Security")
					lblMemberName.Text = "Honorable Member: " + emp.getName();
				else if (emp.getOccupation() == "Disciplinary Committee")
					if (emp.getGender() == 'M')
						lblMemberName.Text = "Mr. " + emp.getName()[0] + ". " + emp.getSurname();
					else
						lblMemberName.Text = "Miss/Mrs. " + emp.getName()[0] + ". " + emp.getSurname();
				else
					MessageBox.Show("ERROR: occupation not found. Please call administrator.");

			}
			else
			{
				MessageBox.Show("ERROR: Employee Object Failed. Please Call Administrator.");
			}
		}

		//Method to create employee object
		private void createEmpObject(int empID)
		{
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT * FROM employee WHERE empID = " + empID;
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						string empNationalID = dataReader.GetValue(1).ToString();
						string name = dataReader.GetValue(2).ToString();
						string surname = dataReader.GetValue(3).ToString();
						char gender = dataReader.GetValue(4).ToString()[0];
						DateTime date = (DateTime)dataReader.GetValue(5);
						string occupation = dataReader.GetValue(6).ToString();
						string phone = dataReader.GetValue(7).ToString();
						string email = dataReader.GetValue(8).ToString();

						emp = new Employee(empNationalID, name, surname, gender, date, phone, email, empID, occupation);
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

		private int getStID(string universityID)
		{
			int stID = -1;

			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT stID FROM student WHERE stUniversityID = " + universityID;
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						stID = (int)dataReader.GetValue(0);
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

			return stID;
		}

		private void btnReportStudents_Click(object sender, EventArgs e)
		{
			if (txtUnivID.Text != "")
			{
				if (cmbMisc.SelectedIndex != -1)
				{
					if(txtLocation.Text != "")
					{
						if (txtDescription.Text != "")
						{
							int stID = getStID(txtUnivID.Text);
							if(stID != -1)
							{
								try
								{
									con.Open();
									if (con.State == ConnectionState.Open)
									{

										//Store the records into the database
										String query = $"INSERT INTO reportedstudent(stID, empID, misconduct, description, location, date, time) VALUES ('" + stID + "','" + emp.getEmpID() + "','" + cmbMisc.SelectedItem.ToString() + "','" + txtDescription.Text + "','" + txtLocation.Text + "','" + DateTime.Today + "','" + DateTime.Now.TimeOfDay + "')";
										MySqlCommand command = new MySqlCommand(query, con);
										dataAdapter = new MySqlDataAdapter();
										dataAdapter.InsertCommand = command;
										if (dataAdapter.InsertCommand.ExecuteNonQuery() == 1)
										{
											//Email The Disciplinary Committee here

											MessageBox.Show("Student Reported Successfully.");
											txtDescription.Text = "";
											txtLocation.Text = "";
											txtUnivID.Text = "";
											cmbMisc.SelectedIndex = -1;
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
								MessageBox.Show("Invalid University ID! Please Try Again.");
							}
						}
						else
						{
							MessageBox.Show("Please enter the Description of the misconduct.");
						}
					}
					else
					{
						MessageBox.Show("Please enter the location where the misconduct occured.");
					}
				}
				else
				{
					MessageBox.Show("Please select the type of misconduct.");
				}
			}
			else
			{
				MessageBox.Show("Please enter the University ID of the Student.");
			}
		}

		private void btnLogOut_Click(object sender, EventArgs e)
		{
			//Closes form and show main form contents
			frmMain.displayMainContents();
			this.Close();
		}

		private void filterByEmp(int empID, DateTime date)
		{
			try
			{

				con.Open();

				//Check if connection was opened successfully
				if (con.State == ConnectionState.Open)
				{

					//Send query to the database
					//date = DateTime.Parse(date.ToString("yyyy-MM-dd"));
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, misconduct, location, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN reportedstudent t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE (t2.date = '" + date + "' AND t2.empID = " + empID + ")";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					gridStudents.DataSource = dataTable; //Display data in the dataGridView

					//Close connections
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
		}

		private void filterAll(DateTime date)
		{
			try
			{

				con.Open();

				//Check if connection was opened successfully
				if (con.State == ConnectionState.Open)
				{

					//Send query to the database
					//date = DateTime.Parse(date.ToString("yyyy-MM-dd"));
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, misconduct, location, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN reportedstudent t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE t2.date = '" + date + "'";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					gridStudents.DataSource = dataTable; //Display data in the dataGridView

					//Close connections
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
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			if (emp.getOccupation() == "Security")
				filterByEmp(emp.getEmpID(), DateTime.Parse(dateTimePicker1.Value.ToString("yyyy-MM-dd")));
			else if (emp.getOccupation() == "Disciplinary Committee")
				filterAll(DateTime.Parse(dateTimePicker1.Value.ToString("yyyy-MM-dd")));
		}

		private void filterByStudent(string universityID, int empID)
		{
			try
			{

				con.Open();

				//Check if connection was opened successfully
				if (con.State == ConnectionState.Open)
				{

					//Send query to the database
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, misconduct, location, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN reportedstudent t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE (t1.stUniversityID LIKE '%" + universityID + "%' AND t2.empID = " + empID + ")";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					gridStudents.DataSource = dataTable; //Display data in the dataGridView

					//Close connections
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
		}

		private void filterByStudentAll(string universityID)
		{
			try
			{

				con.Open();

				//Check if connection was opened successfully
				if (con.State == ConnectionState.Open)
				{

					//Send query to the database
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, misconduct, location, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN reportedstudent t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE t1.stUniversityID LIKE '%" + universityID + "%'";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					gridStudents.DataSource = dataTable; //Display data in the dataGridView

					//Close connections
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
		}

		private void txtUniversityID2_TextChanged(object sender, EventArgs e)
		{
			if (emp.getOccupation() == "Security")
				filterByStudent(txtUniversityID2.Text, emp.getEmpID());
			else if (emp.getOccupation() == "Disciplinary Committee")
				filterByStudentAll(txtUniversityID2.Text);
		}

		private void gridStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int index = e.RowIndex;
			DataGridViewRow row = gridStudents.Rows[index];
			string stUniversityID = row.Cells[0].Value.ToString();
			int stID = getStID(stUniversityID);
			frmMain.openStudentProfile(stID);
		}

		private void rbAsending_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAsending.Checked)
			{
				gridStudents.Sort(gridStudents.Columns[1], ListSortDirection.Ascending);
			}
			else if (rbDescending.Checked)
			{
				gridStudents.Sort(gridStudents.Columns[1], ListSortDirection.Descending);
			}
		}

		private bool printReport()
		{
			bool printed = false;

			//Make method of type string and return the file name
			//PrintDialog printDialog = new PrintDialog();
			//printDialog.Document = printDocumentNow;
			printMisc.Document = printReportedStud;

			//DialogResult result = printNow.ShowDialog();
			if (printMisc.ShowDialog() == DialogResult.OK)
			{
				printReportedStud.Print();
				printed = true;
			}

			return printed;
		}

		private void btnPrintReport_Click(object sender, EventArgs e)
		{
			int counter = 0;

			while (counter < gridStudents.RowCount)
			{
				string universityID = gridStudents.Rows[counter].Cells[0].FormattedValue.ToString();
				string name = gridStudents.Rows[counter].Cells[1].FormattedValue.ToString();
				string surname = gridStudents.Rows[counter].Cells[2].FormattedValue.ToString();
				string course = gridStudents.Rows[counter].Cells[3].FormattedValue.ToString();
				string AcademicYear = gridStudents.Rows[counter].Cells[4].FormattedValue.ToString();
				string misconduct = gridStudents.Rows[counter].Cells[5].FormattedValue.ToString();
				string location = gridStudents.Rows[counter].Cells[6].FormattedValue.ToString();
				string empSurname = gridStudents.Rows[counter].Cells[9].FormattedValue.ToString();

				reportContent += "\n" + String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15} {7,-15}", universityID, name, surname, course, AcademicYear, misconduct, location, empSurname);
				counter++;
			}

			if (reportContent != "")
			{
				reportContent = "\n\n" + reportContent;
				if (printReport()) MessageBox.Show("Misconduct Report printed successfully.");
			}
			else
			{
				MessageBox.Show("No Misconduct Information Found. Please filter the information of available reported students to print.");
			}
		}

		private void printReportedStud_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawString("Reported Students", new Font("Arial", 40, FontStyle.Underline), Brushes.Black, new Point(200, 10));
			e.Graphics.DrawString(dateTimePicker1.Value.ToString("yyyy-MM-dd"), new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(350, 100));
			e.Graphics.DrawString(String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15} {7,-15}", "University ID", "Name", "Surname", "Course", "Academic Year", "Misconduct", "Location", "Employee"), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(20, 175));
			e.Graphics.DrawString(reportContent + "\n\nNumber of Misconducts: " + gridStudents.RowCount, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 175));
		}
	}
}
