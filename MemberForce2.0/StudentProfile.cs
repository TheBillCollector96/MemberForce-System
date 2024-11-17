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
	public partial class StudentProfile : Form
	{
		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");

		public StudentProfile(int stID)
		{
			InitializeComponent();
			fillStudentDetails(stID);
		}

		private void fillStudentDetails(int stID)
		{
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT stNationalID, stName, stSurname, stGender, stDateOfBirth, stCourse, stAcademicYear, stPhone, stEmail, stUniversityID, misconduct, description, location, date, time FROM student t1 INNER JOIN reportedstudent t2 ON t1.stID = t2.stID WHERE t1.stID = " + stID;
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						txtNationalID.Text = dataReader.GetValue(0).ToString();
						txtName.Text = dataReader.GetValue(1).ToString();
						txtSurname.Text = dataReader.GetValue(2).ToString();
						char gender = dataReader.GetValue(3).ToString()[0];
						if (gender == 'M')
							txtGender.Text = "Male";
						else
							txtGender.Text = "Female";
						DateTime date = (DateTime)dataReader.GetValue(4);
						txtDateOfBirth.Text = date.ToString("yyyy-MM-dd");
						txtCourse.Text = dataReader.GetValue(5).ToString();
						txtAcademicYear.Text = dataReader.GetValue(6).ToString();
						txtPhone.Text = dataReader.GetValue(7).ToString();
						txtEmail.Text = dataReader.GetValue(8).ToString();
						txtUniversityID.Text = dataReader.GetValue(9).ToString();
						txtMisconduct.Text = dataReader.GetValue(10).ToString();
						txtDescription.Text = dataReader.GetValue(11).ToString();
						txtLocation.Text = dataReader.GetValue(12).ToString();
						date = (DateTime)dataReader.GetValue(13);
						txtDateOfMisc.Text = date.ToString("yyyy-MM-dd");
						txtTime.Text = dataReader.GetValue(14).ToString();

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

		private bool printReport()
		{
			bool printed = false;

			//Make method of type string and return the file name
			//PrintDialog printDialog = new PrintDialog();
			//printDialog.Document = printDocumentNow;
			printDialogProfile.Document = printStudentProfile;

			//DialogResult result = printNow.ShowDialog();
			if (printDialogProfile.ShowDialog() == DialogResult.OK)
			{
				printStudentProfile.Print();
				printed = true;
			}

			return printed;
		}

		private void btnPrintMisconduct_Click(object sender, EventArgs e)
		{
			//Print Student Profile
			if (printReport()) MessageBox.Show("Permit Report printed successfully.");
		}

		private void printStudentProfile_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawString("Reported Student", new Font("Arial", 40, FontStyle.Underline), Brushes.Black, new Point(200, 10)); 
			e.Graphics.DrawString(String.Format("{0,-15}", "Student Details:") + "\n"
								+ String.Format("{0,-15} {1,-15}", "University ID:", txtUniversityID.Text) + "\n"
								 + String.Format("{0,-15} {1,-15}", "National ID:", txtNationalID.Text) + "\n"
								 + String.Format("{0,-15} {1,-15}", "Name:", txtName.Text) + "\n"
								 + String.Format("{0,-15} {1,-15}", "Surname:", txtSurname.Text) + "\n"
								 + String.Format("{0,-15} {1,-15}", "Gender:", txtGender.Text) + "\n" 
								 + String.Format("{0,-15} {1,-15}", "Date of Birth:", txtDateOfBirth.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Course:", txtCourse.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Academic Year:", txtAcademicYear.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Phone:", txtPhone.Text) + "\n"
								 + String.Format("{0,-15} {1,-15}", "Email:", txtEmail.Text) + "\n\n"
								 + String.Format("{0,-15}", "Misconduct Details:") + "\n"
								+ String.Format("{0,-15} {1,-15}", "Date of Misconduct:", txtDateOfMisc.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Time of Misconduct:", txtTime.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Misconduct:", txtMisconduct.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Location:", txtLocation.Text) + "\n"
								+ String.Format("{0,-15} {1,-15}", "Description:", txtDescription.Text)
								 , new Font("Arial", 15, FontStyle.Regular), Brushes.Black, new Point(20, 175));
		}
	}
}
