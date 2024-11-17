using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemberForce2._0
{
	public partial class TemporaryPermitGenerator : Form
	{

		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");
		MySqlDataAdapter dataAdapter;
		DataSet dataSet;
		DataTable dataTable;

		private static Employee emp; //Used to keep track of current signed in employee
		private static Student student;
		private static string reportContent;
		private MemberForce frmMain;

		public TemporaryPermitGenerator(MemberForce frmMain, int empID)
		{
			InitializeComponent();
			this.frmMain = frmMain;
			createEmpObject(empID);
			if (emp != null)
			{
				if (emp.getOccupation() == "Security")
					lblMemberName.Text = "Honorable Member: " + emp.getName();
				else if (emp.getOccupation() == "NWU Admin")
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

		private void createNewStudent(int stID)
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
						string stNationalID = dataReader.GetValue(1).ToString();
						string stName = dataReader.GetValue(2).ToString();
						string stSurname = dataReader.GetValue(3).ToString();
						char stGender = dataReader.GetValue(4).ToString()[0];
						DateTime stDateOfBirth = (DateTime)dataReader.GetValue(5);
						string stCourse = dataReader.GetValue(6).ToString();
						int stAcademicYear = (int)dataReader.GetValue(7);
						string stPhone = dataReader.GetValue(8).ToString();
						string stEmail = dataReader.GetValue(9).ToString();
						string stUniversityID = dataReader.GetValue(10).ToString();
						byte[] arrImage = (byte[])dataReader.GetValue(12);

						//Create Student Object
						student = new Student(stNationalID, stName, stSurname, stGender, stDateOfBirth, stPhone, stEmail, stID, stCourse, stAcademicYear, stUniversityID, arrImage);

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

		private bool printPermit()
		{
			bool printed = false;

			//Make method of type string and return the file name
			PrintDialog printDialog = new PrintDialog();
			//printDialog.Document = printDocumentNow;
			printNow.Document = printDocumentNow;

			//DialogResult result = printNow.ShowDialog();
			if (printNow.ShowDialog() == DialogResult.OK)
			{
				printDocumentNow.Print();
				printed = true;
			}

			return printed;
		}

		//Counts the attempts made by student
		private bool countAttempts(int stID, DateTime dateToday)
		{
			bool available = true;
			int attempts = 5;
			DateTime monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT * FROM generatedpermits WHERE stID = " + stID + " AND (date BETWEEN '" + monthStart + "' AND '" + dateToday + "')";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						attempts--;
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

			if (attempts <= 0) available = false;

			return available;
		}

		private void registerPermit(string gateNum)
		{
			//Student get 5 attempts each moth
			if (countAttempts(student.getStID(), DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"))))
			{
				try
				{
					con.Open();
					if (con.State == ConnectionState.Open)
					{

						//Store the records into the database
						String query = $"INSERT INTO generatedpermits(stID, empID, gateNumber, date, time) VALUES ('" + student.getStID() + "','" + emp.getEmpID() + "','" + gateNum + "','" + DateTime.Today + "','" + DateTime.Now.TimeOfDay + "')";
						MySqlCommand command = new MySqlCommand(query, con);
						dataAdapter = new MySqlDataAdapter();
						dataAdapter.InsertCommand = command;
						if (dataAdapter.InsertCommand.ExecuteNonQuery() == 1)
						{

							if (printPermit())
							{
								MessageBox.Show("Student Permit Printed Successfully.");
								txtUnivID.Text = "";
								txtName.Text = "";
								txtSurname.Text = "";
								txtGender.Text = "";
								txtCourse.Text = "";
								txtAcademicYear.Text = "";
								picStudentID.Image = picStudentID.InitialImage;
							}
							else
							{
								MessageBox.Show("ERROR: Failed to Print Student Permit. Call Administrator.");
							}

						}
						else
						{
							MessageBox.Show("Failed to Register Permit. \nCall Administrator");
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
				MessageBox.Show("Unfortunately you have used all 5 attempts this month.");
			}
			
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			if (txtName.Text != "" && txtSurname.Text != "" && txtGender.Text != "" && txtCourse.Text != "" && txtAcademicYear.Text != "")
			{
				if (rbG1.Checked)
					registerPermit("G1");
				else if (rbG2.Checked)
					registerPermit("G2");
				else if (rbG3.Checked)
					registerPermit("G3");
				else if (rbG4.Checked)
					registerPermit("G4");
				else
					MessageBox.Show("Please select the Gate Number you're currently printing permit from.");
			}
			else
			{
				MessageBox.Show("Please search for the student you'd like to print permit for using their University ID first.");
			}
		}

		private Image getImage(byte[] arrImage)
		{
			MemoryStream stream = new MemoryStream(arrImage);
			return Image.FromStream(stream);
		}

		private void fillStudentDetails()
		{
			txtName.Text = student.getName();
			txtSurname.Text = student.getSurname();

			if (student.getGender() == 'M')
				txtGender.Text = "Male";
			else
				txtGender.Text = "Female";

			txtCourse.Text = student.getCourse();
			txtAcademicYear.Text = student.getAcademicYear().ToString();
			picStudentID.Image = getImage(student.getArrImage());

		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (txtUnivID.Text != "")
			{
				int stID = getStID(txtUnivID.Text);
				if (stID != -1)
				{
					createNewStudent(stID);
					if (student != null)
						fillStudentDetails();
					else
						MessageBox.Show("Failed to create New Student. Please call administrator.");
				}
				else
				{
					MessageBox.Show("Student not found. Please try again.");
				}
			}
			else
			{
				MessageBox.Show("Please enter the University ID of the student you wish to search.");
			}
		}

		private void btnLogout_Click(object sender, EventArgs e)
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
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, gateNumber, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN generatedpermits t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE (t2.date = '" + date + "' AND t2.empID = " + empID + ")";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					dgPermits.DataSource = dataTable; //Display data in the dataGridView

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
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, gateNumber, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN generatedpermits t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE t2.date = '" + date + "'";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					dgPermits.DataSource = dataTable; //Display data in the dataGridView

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

		private void permitDate_ValueChanged(object sender, EventArgs e)
		{
			if (emp.getOccupation() == "Security")
				filterByEmp(emp.getEmpID(), DateTime.Parse(permitDate.Value.ToString("yyyy-MM-dd")));
			else if (emp.getOccupation() == "NWU Admin")
				filterAll(DateTime.Parse(permitDate.Value.ToString("yyyy-MM-dd")));
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
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, gateNumber, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN generatedpermits t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE (t1.stUniversityID LIKE '%" + universityID + "%' AND t2.empID = " + empID + ")";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					dgPermits.DataSource = dataTable; //Display data in the dataGridView

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
					String query = "SELECT stUniversityID, stName, stSurname, stCourse, stAcademicYear, gateNumber, date, empName, empSurname "
								+ "FROM student t1 INNER JOIN generatedpermits t2 ON t1.stID = t2.stID INNER JOIN employee t3 ON t2.empID = t3.empID WHERE t1.stUniversityID LIKE '%" + universityID + "%'";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter(command);
					dataTable = new DataTable();

					dataAdapter.Fill(dataTable); //Retrieves the data from the database
					dgPermits.DataSource = dataTable; //Display data in the dataGridView

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

		private void txtUniversityID_TextChanged(object sender, EventArgs e)
		{
			if (emp.getOccupation() == "Security")
				filterByStudent(txtUniversityID.Text, emp.getEmpID());
			else if (emp.getOccupation() == "NWU Admin")
				filterByStudentAll(txtUniversityID.Text);
		}

		private void printDocumentNow_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawString("Temporary Permit", new Font("Arial", 40, FontStyle.Underline), Brushes.Black, new Point(200, 10));
			e.Graphics.DrawString(DateTime.Today.ToString("yyyy-MM-dd"), new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(350, 100));
			/*e.Graphics.DrawString("University ID:\t" + student.getUniversityID(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 40));
			e.Graphics.DrawString("\nName:\t" + student.getName(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 50));
			e.Graphics.DrawString("\nSurname:\t" + student.getSurname(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 60));
			e.Graphics.DrawString("\nGender:\t" + student.getGender(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 70));
			e.Graphics.DrawString("\nCourse:\t" + student.getCourse(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 80));
			e.Graphics.DrawString("\nAcademic Year:\t" + student.getAcademicYear(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 90));
			e.Graphics.DrawString(("\n\nThis permit grants " + student.getName()[0] + ". " + student.getSurname()
								+ " access around NWU VTC on " + DateTime.Today.ToString("yyyy-MM-dd")), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 100));*/
			e.Graphics.DrawString(String.Format("{0,-15} {1,-15}", "National ID:", student.getUniversityID()) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Name:", student.getName()) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Surname:", student.getSurname()) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Gender:", student.getGender()) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Course:", student.getCourse()) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Academic Year:", student.getAcademicYear())
								+ "\n\nThis permit grants " + student.getName()[0] + ". " + student.getSurname()
								+ " access around NWU VTC on " + DateTime.Today.ToString("yyyy-MM-dd")
								, new Font("Arial", 15, FontStyle.Regular), Brushes.Black, new Point(20, 175));
		}

		private void rbAsending_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAsending.Checked)
			{
				dgPermits.Sort(dgPermits.Columns[1], ListSortDirection.Ascending);
			}
			else if (rbDescending.Checked)
			{
				dgPermits.Sort(dgPermits.Columns[1], ListSortDirection.Descending);
			}
		}

		private bool printReport()
		{
			bool printed = false;

			//Make method of type string and return the file name
			//PrintDialog printDialog = new PrintDialog();
			//printDialog.Document = printDocumentNow;
			printNow.Document = printPermitReport;

			//DialogResult result = printNow.ShowDialog();
			if (printNow.ShowDialog() == DialogResult.OK)
			{
				printPermitReport.Print();
				printed = true;
			}

			return printed;
		}

		private void btnPrintReport_Click(object sender, EventArgs e)
		{
			int counter = 0;

			while (counter < dgPermits.RowCount)
			{
				string universityID = dgPermits.Rows[counter].Cells[0].FormattedValue.ToString();
				string name = dgPermits.Rows[counter].Cells[1].FormattedValue.ToString();
				string surname = dgPermits.Rows[counter].Cells[2].FormattedValue.ToString();
				string course = dgPermits.Rows[counter].Cells[3].FormattedValue.ToString();
				string AcademicYear = dgPermits.Rows[counter].Cells[4].FormattedValue.ToString();
				string gateNumber = dgPermits.Rows[counter].Cells[5].FormattedValue.ToString();
				string empSurname = dgPermits.Rows[counter].Cells[8].FormattedValue.ToString();

				reportContent += "\n" + String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}", universityID, name, surname, course, AcademicYear, gateNumber, empSurname);
				counter++;
			}

			if (reportContent != "")
			{
				reportContent = "\n\n" + reportContent;
				if (printReport()) MessageBox.Show("Permit Report printed successfully.");
			}
			else
			{
				MessageBox.Show("No Permit Information Found. Please filter the information of available permits to print.");
			}
		}

		private void printPermitReport_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawString("Permit Report", new Font("Arial", 40, FontStyle.Underline), Brushes.Black, new Point(200, 10));
			e.Graphics.DrawString(permitDate.Value.ToString("yyyy-MM-dd"), new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(350, 100));
			e.Graphics.DrawString(String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}", "University ID", "Name", "Surname", "Course", "Academic Year", "Gate No.", "Employee"), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(20, 175));
			e.Graphics.DrawString(reportContent + "\n\nNumber of Permits: " + dgPermits.RowCount, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 175));
		}
	}
}
