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
    public partial class StudentResidenceVisitor : Form
    {

		System.Timers.Timer timer = new System.Timers.Timer();

		//Establish connection to the database
		//SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
		MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");
		MySqlDataAdapter dataAdapter;
        DataSet dataSet;
		DataTable dataTable;

        private static Employee emp; //Used to keep track of current signed in employee
		private int size = 0;
		private const int maxVisitors = 100;
		private const int maxResidences = 100;
		private static MyArrayList<ResidenceStudent> listResStudents = new MyArrayList<ResidenceStudent>(maxVisitors); //Used by Security
		private static MyArrayList<ResidenceStudent> listResStudents2; //Used by House Committee
		private static MyArrayList<Visitor> listVisitors = new MyArrayList<Visitor>(maxVisitors); //Used by Security
		private static MyArrayList<Residence> listResidences = new MyArrayList<Residence>(maxResidences);
		private MemberForce frmMain;
		private static int currentResID = -1;
		private static string reportContent = "";

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

        public StudentResidenceVisitor(MemberForce frmMain, int empID)
		{
			//Inititialise Class Objects and Controls
			InitializeComponent();
			this.frmMain = frmMain;
			fillResidences();
			cmbResVisitor.Enabled = true;

			//Initialise Employee Object attributes
			createEmpObject(empID);
			if (emp != null)
			{
				if (emp.getOccupation() == "Security")
				{
					lblMemberName.Text = "Honorable Member: " + emp.getName();
					tabResStudentVisitor.SelectedTab = tabMaintainVisitor;
				}
				else if (emp.getOccupation() == "House Committee")
				{
					if (emp.getGender() == 'M')
					{
						lblMemberName.Text = "Mr. " + emp.getName()[0] + ". " + emp.getSurname();
					}
					else
					{
						lblMemberName.Text = "Miss/Mrs. " + emp.getName()[0] + ". " + emp.getSurname();
					}
					tabResStudentVisitor.SelectedTab = tabViewReport;
				}
				else
				{
					MessageBox.Show("ERROR: occupation not found. Please call administrator.");
				}
			}
			else
			{
				MessageBox.Show("ERROR: Employee Object Failed. Please Call Administrator.");
			}
        }

		public void updateVisitorList(Visitor visitor, int index)
        {
			listVisitors.update(index, visitor);
			refreshVisitors();
        }

		private int authenticateResStudent(string username, string password)
		{
			int stID = -1;

			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT username, password, stID FROM residencestudent";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						if (dataReader.GetValue(0).ToString() == username && dataReader.GetValue(1).ToString() == password)
						{
							stID = (int)dataReader.GetValue(2);
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

			return stID;
		}

		private ResidenceStudent createNewResStudent(int stID)
		{
			ResidenceStudent student = null;

			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT student.stID, student.stNationalID, student.stName, student.stSurname, student.stGender, student.stDateOfBirth, student.stCourse, student.stAcademicYear, student.stPhone, student.stEmail, student.stUniversityID, residencestudent.roomNumber, residencestudent.monthlyVisits, residencestudent.visitorRestricted, residencestudent.resID FROM student INNER JOIN residencestudent ON student.stID = residencestudent.stID";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						if((int)dataReader.GetValue(0) == stID)
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
							string roomNumber = dataReader.GetValue(11).ToString();
							int monthlyVisits = (int)dataReader.GetValue(12);
							bool visitorRestricted = (bool)dataReader.GetValue(13);
							int resID = (int)dataReader.GetValue(14);

							//Creates new Residence Student Object
							student = new ResidenceStudent(stNationalID, stName, stSurname, stGender, stDateOfBirth, stPhone, stEmail, stID, stCourse, stAcademicYear, stUniversityID, roomNumber, monthlyVisits, visitorRestricted, resID);
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

			return student;
		}

		private Visitor createVisitor(int stID, int empID, string nationalID, string name, string surname, string phone, string email, DateTime date, TimeSpan timeIN)
        {
			Visitor visitor = null;
			bool inserted = false;

			//Insert new Visitor data
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{

					//Store the records into the database
					String query = $"INSERT INTO visitors(stID, empID, nationalID, name, surname, phone, email, date, timeIN) VALUES ('" + stID + "','" + empID + "','" + nationalID + "','" + name + "','" + surname + "','" + phone + "','" + email + "','" + date + "','" + timeIN +  "')";
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter();
					dataAdapter.InsertCommand = command;
					if (dataAdapter.InsertCommand.ExecuteNonQuery() == 1)
					{
						inserted = true;
					}
					else
					{
						MessageBox.Show("Order Failed. \nCall Administrator");
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

			//gets the visitorID
			if (inserted)
			{
				int visitorID = -1;
				try
				{
					con.Open();
					if (con.State == ConnectionState.Open)
					{
						String query = "SELECT stID, empID, date, nationalID, visitorID FROM visitors";
						MySqlCommand command = new MySqlCommand(query, con);
						MySqlDataReader dataReader = command.ExecuteReader();
						while (dataReader.Read())
						{
							if ((int)dataReader.GetValue(0) == stID && (int)dataReader.GetValue(1) == empID && dataReader.GetValue(2).ToString() == DateTime.Today.ToString() && dataReader.GetValue(3).ToString() == nationalID)
							{
								visitorID = (int)dataReader.GetValue(4);
								visitor = new Visitor(name, surname, nationalID, phone, email, timeIN, visitorID);
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
			}

			return visitor;
        }
		private ResidenceStudent incrementMonthlyVisits(ResidenceStudent student)
		{
			
			//Increment from ArrayList
			int visits = student.getMonthlyVisits();
			visits++; //Increment available students
			student.setMonthlyVisits(visits);

			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					//Updates the monthlyVisits in the database
					String query = "UPDATE residencestudent SET monthlyVisits = " + visits + " WHERE stID = " + student.getStID();
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter();
					dataAdapter.UpdateCommand = command;
					if (dataAdapter.UpdateCommand.ExecuteNonQuery() != 1) MessageBox.Show("ERROR: Failed to update database. Please call administrator."); 
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

			return student;
		}
		private void btnSignInVisitor_Click(object sender, EventArgs e)
		{
			//Check if it's not late to sign in visitor
			if(txtName.Text != "")
			{
				if(txtSurname.Text != "")
				{
					if(txtNationalID.Text != "")
					{
						if(txtEmail.Text != "")
						{
							if(txtPhone.Text != "")
							{
								if(txtusername.Text != "")
                                {
									if(txtPassword.Text != "")
                                    {
										//Ensure Residence was chosen to operate on
										if(currentResID != -1)
										{
											//Authenticate Residence Student
											int stID = authenticateResStudent(txtusername.Text, txtPassword.Text);
											if (stID != -1)
											{
												//Create Residence Student Object
												ResidenceStudent student = createNewResStudent(stID);
												if (student != null)
												{
													//Ensures the Student is from the given residence before signing in visitors
													if (student.getResID() == currentResID)
													{

														//Validates if Student is allowed to Sign in Visitors
														if (!student.getVisitorRestriction())
														{
															//Create and Store Visitor Object in the database
															Visitor visitor = createVisitor(student.getStID(), emp.getEmpID(), txtNationalID.Text, txtName.Text, txtSurname.Text, txtPhone.Text, txtEmail.Text, DateTime.Today, DateTime.Now.TimeOfDay);
															if (visitor != null)
															{
																student = incrementMonthlyVisits(student);
																//Adds Student and Visitor Object to their respective Generic ArrayList
																listVisitors.add(size, visitor);
																listResStudents.add(size, student);
																size++; //Increment the Size of the Generic ArrayList
																refreshVisitors(); //Refresh the list of current visitors
																MessageBox.Show("Visitor " + visitor.getName() + " has been signed in by " + student.getName() + " successfully.");
																txtName.Text = "";
																txtSurname.Text = "";
																txtNationalID.Text = "";
																txtPhone.Text = "";
																txtEmail.Text = "";
																txtusername.Text = "";
																txtPassword.Text = "";
															}
															else
															{
																MessageBox.Show("ERROR: Visitor object failed. Please call administrator.");
															}
														}
														else
														{
															MessageBox.Show("Unfortunately you have been Restricted from signing in visitors. Please consult with House Committee.");
														}
													}
													else
													{
														MessageBox.Show("Unfortunately you are not a Residence Student of " + cmbResVisitor.SelectedItem.ToString());
													}
												}
												else
												{
													MessageBox.Show("ERROR: Student object failed. Please call administrator.");
												}
											}
											else
											{
												MessageBox.Show("Incorrect Username and Password. Please try again.");
											}
										}
										else
										{
											tabResStudentVisitor.SelectedTab = tabMaintainVisitor;
											MessageBox.Show("Please Select a Residence On-duty to Sign In Visitors.");
										}
                                    }
                                    else
                                    {
										MessageBox.Show("Please enter the password of the Residence Student.");
                                    }
                                }
								else
								{
									MessageBox.Show("Please enter the Username of the Residence Student.");
								}
							}
							else
							{
								MessageBox.Show("Please enter the visitor's Phone.");
							}
						}
						else
						{
							MessageBox.Show("Please enter the visitor's Email.");
						}
					}
					else
					{
						MessageBox.Show("Please enter the visitor's National ID.");
					}
				}
				else
				{
					MessageBox.Show("Please enter the Surname of the visitor.");
				}
			}
			else
			{
				MessageBox.Show("Please enter the Name of the visitor.");
			}
		}

		//Refresh ListBoxes
		private void refreshVisitors()
        {
			lstVisitors.Items.Clear();
			//lstVisitors.Items.Add("Name \t Surname \t Visited Student \t Room No. \t Time IN");
			for(int i = 0; i < size; i++)
            {
				
				lstVisitors.Items.Add(String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15}", listVisitors.get(i).getName(), listVisitors.get(i).getSurname(), listResStudents.get(i).getName()[0] + ". " + listResStudents.get(i).getSurname(), listResStudents.get(i).getRoomNo(), listVisitors.get(i).getTimeIN()));
				//listVisitors.get(i).getName() + " \t " + listVisitors.get(i).getSurname() + " \t " + listResStudents.get(i).getName()[0] + ". " + listResStudents.get(i).getSurname() + " \t " + listResStudents.get(i).getRoomNo() + " \t " + listVisitors.get(i).getTimeIN()
			}
		}

		//Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
			lblClock.Text = DateTime.Now.ToString("T");
        }

        private void StudentResidenceVisitor_Load(object sender, EventArgs e)
        {
			//timer1.Start();
			timer.Interval = 1000;
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
			refreshVisitors();
        }

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
			Invoke(new MethodInvoker(delegate () { lblClock.Text = DateTime.Now.ToString("T"); }));
        }

        private void btnEditVisitors_Click(object sender, EventArgs e)
        {
			if(lstVisitors.SelectedIndex != -1)
            {
				frmMain.openEditVisitor(this, listVisitors.get(lstVisitors.SelectedIndex), emp.getEmpID(), listResStudents.get(lstVisitors.SelectedIndex).getStID(), lstVisitors.SelectedIndex);
            }
            else
            {
				MessageBox.Show("Please select the visitor you'd like to edit.");
            }
        }

		private void btnSignOutVisitors_Click(object sender, EventArgs e)
		{
			if(lstVisitors.SelectedIndex != -1)
			{
				try
				{
					con.Open();
					if (con.State == ConnectionState.Open)
					{
						//Updates the timeOUT in the database
						String query = "UPDATE visitors SET timeOUT = '" + DateTime.Now.TimeOfDay + "' WHERE visitorID = " + listVisitors.get(lstVisitors.SelectedIndex).getVisitorID();
						MySqlCommand command = new MySqlCommand(query, con);
						dataAdapter = new MySqlDataAdapter();
						dataAdapter.UpdateCommand = command;
						if (dataAdapter.UpdateCommand.ExecuteNonQuery() == 1)
						{
							//Remove the visitor from the Generic Array
							Visitor visitor = listVisitors.remove(lstVisitors.SelectedIndex);
							ResidenceStudent student = listResStudents.remove(lstVisitors.SelectedIndex);
							size--; //
							refreshVisitors();
							MessageBox.Show("Good Bye " + visitor.getName() + ". :) Thanks for visiting " + student.getName());
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
			}
		}

		private void tabResStudentVisitor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(emp.getOccupation() != "House Committee" && tabResStudentVisitor.SelectedTab == tabViewReport)
			{
				tabResStudentVisitor.SelectedTab = tabMaintainVisitor;
				MessageBox.Show("Appologies " + emp.getName()[0] + ". " + emp.getSurname() + ", you don't have current access.");
			}
			else if (emp.getOccupation() != "House Committee" && tabResStudentVisitor.SelectedTab == tabResStudent)
			{
				tabResStudentVisitor.SelectedTab = tabMaintainVisitor;
				MessageBox.Show("Appologies " + emp.getName()[0] + ". " + emp.getSurname() + ", you don't have current access.");
			}

			if (tabResStudentVisitor.SelectedTab == tabSignInVisitor)
			{
				lblHelp.Text = "Enter Visitor's Details and Authenticate Student to sign in visitor";
			}
			else if (tabResStudentVisitor.SelectedTab == tabMaintainVisitor)
			{
				lblHelp.Text = "Click here to refresh Current Visitors";
			}
			else if(tabResStudentVisitor.SelectedTab == tabViewReport)
			{
				lblHelp.Text = "Choose a Date to filter Signed In Visitors";
			}
			else if(tabResStudentVisitor.SelectedTab == tabResStudent)
			{
				lblHelp.Text = "Choose a Residence to view its current students";
			}
		}

		private int getResIDFromArrayList(string residenceName)
		{
			int counter = 0;
			int resID = -1;

			//Filters through the ArrayList to find the resID based on the given Residence Name
			while (counter < listResidences.getSize())
			{
				if (listResidences.get(counter).getResName() == residenceName) resID = listResidences.get(counter).getResID();
				counter++;
			}

			return resID;
		}

		private void filterVisitor(DateTime date, string residenceName) 
		{
			if (residenceName == "ALL")
			{
				try
				{

					con.Open();

					//Check if connection was opened successfully
					if (con.State == ConnectionState.Open)
					{
						//Send query to the database
						String query = "SELECT name, surname, date, timeIN, timeOUT, stName, stSurname, roomNumber, empName, empSurname "
									+ "FROM student t1 INNER JOIN residencestudent t2 ON t1.stID = t2.stID INNER JOIN visitors t3 ON t2.stID = t3.stID INNER JOIN employee t4 ON t3.empID = t4.empID WHERE t3.date = '" + date + "'";
						MySqlCommand command = new MySqlCommand(query, con);
						dataAdapter = new MySqlDataAdapter(command);
						dataTable = new DataTable();

						dataAdapter.Fill(dataTable); //Retrieves the data from the database
						dgVisitors.DataSource = dataTable; //Display data in the dataGridView

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
			else
			{
				//Get resID based on selected residence name
				int resID = getResIDFromArrayList(residenceName);

				if(resID != -1)
				{
					try
					{

						con.Open();

						//Check if connection was opened successfully
						if (con.State == ConnectionState.Open)
						{
							//Send query to the database
							String query = "SELECT name, surname, date, timeIN, timeOUT, stName, stSurname, roomNumber, empName, empSurname "
										+ "FROM student t1 INNER JOIN residencestudent t2 ON t1.stID = t2.stID INNER JOIN visitors t3 ON t2.stID = t3.stID INNER JOIN employee t4 ON t3.empID = t4.empID WHERE (t3.date = '" + date + "' AND t2.resID = " + resID + ")";
							MySqlCommand command = new MySqlCommand(query, con);
							dataAdapter = new MySqlDataAdapter(command);
							dataTable = new DataTable();

							dataAdapter.Fill(dataTable); //Retrieves the data from the database
							dgVisitors.DataSource = dataTable; //Display data in the dataGridView

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
				else
				{
					MessageBox.Show("ERROR: Invalid Residence Name. Please Call Administrsator");
				}
			}
		}

		private void dateFilterVisitors_ValueChanged(object sender, EventArgs e)
		{
			if (cmbFilterRes.SelectedIndex != -1)
			{
				string date = dateFilterVisitors.Value.ToString("yyyy-MM-dd");
				filterVisitor(DateTime.Parse(date), cmbFilterRes.SelectedItem.ToString());
			}
			else
			{
				MessageBox.Show("Please select the Residence you would like to filter from.");
			}
		}

		private void btnLogOut_Click(object sender, EventArgs e)
		{
			if (lstVisitors.Items.Count == 0)
			{
				listResidences.clearList();
				listResStudents.clearList();
				if (listResStudents2 != null) listResStudents2.clearList();
				listVisitors.clearList();
				timer.Stop();
				timer.Close();
				frmMain.displayMainContents();
				this.Close();
			}
			else
			{
				MessageBox.Show("Please make sure all visitors are signed out first.");
			}
			
		}

		public void fillResidences()
		{
			int count = 0;
			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT resID, resName, resStreet, resCity, resZip, numOfRooms, availableStudents FROM residence";
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						int resID = (int)dataReader.GetValue(0);
						string resName = dataReader.GetValue(1).ToString();
						string resStreet = dataReader.GetValue(2).ToString();
						string resCity = dataReader.GetValue(3).ToString();
						string resZip = dataReader.GetValue(4).ToString();
						int numOfRooms = (int)dataReader.GetValue(5);
						int availableStudents = (int)dataReader.GetValue(6);

						//Creates new Residence Object 
						Residence residence = new Residence(resID, resName, resStreet, resCity, resZip, numOfRooms, availableStudents);
						cmbResidences.Items.Add(residence.getResName()); //Adds the Name of Residence Object into the listbox
						cmbResVisitor.Items.Add(residence.getResName());
						listResidences.add(count, residence); //Adds the new object into Residence ArrayList
						count++;
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

		public int [] getFilterStID(int resID, int numOfRooms)
		{
			int[] arrStID = new int [numOfRooms];
			int count = 0;

			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					String query = "SELECT stID FROM residencestudent WHERE resID = " + resID;
					MySqlCommand command = new MySqlCommand(query, con);
					MySqlDataReader dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						arrStID[count] = (int)dataReader.GetValue(0);
						count++;
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

			return arrStID;

		}

		public void updateResidence(int index, Residence residence)
		{
			listResidences.update(index, residence);
		}

		public void refreshResStudents(int index)
		{
			lstStudents.Items.Clear();
			lblAvailableRooms.Text = (listResidences.get(index).getNumOfRooms() - listResidences.get(index).getAvailableStu()).ToString();
			//Filters student IDs of Students who stay at the selected Residence
			int[] arrStID = getFilterStID(listResidences.get(index).getResID(), listResidences.get(index).getNumOfRooms());
			//creates objects of available Residence students of the selected Residence
			listResStudents2 = new MyArrayList<ResidenceStudent>(listResidences.get(index).getNumOfRooms()); //Limits Generic ArrayList to the number of rooms
			//lstStudents.Items.Add("Name \t Surname \t Room No. \t Monthly Visits \t Visitor Restricted");
			for (int i = 0; arrStID[i] != 0; i++)
			{
				ResidenceStudent student = createNewResStudent(arrStID[i]);
				//MessageBox.Show(arrStID.Length.ToString());
				if (student != null)
				{
					//student.getName() + " \t " + student.getSurname() + " \t " + student.getRoomNo() + " \t " + student.getMonthlyVisits() + " \t " + student.getVisitorRestriction()
					
					lstStudents.Items.Add(String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15}", student.getName(), student.getSurname(), student.getRoomNo(), student.getMonthlyVisits(), student.getVisitorRestriction())); //Adds Student Object's details to the listbox
					listResStudents2.add(i, student); //Adds new Created Residence Student Object to the Generic Array List
				}


			}
		}

		private void cmbResidences_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(cmbResidences.SelectedIndex != -1)
			{
				refreshResStudents(cmbResidences.SelectedIndex);
			}
			
		}

		private bool changeRestriction(int restriction, int stID, int index)
		{
			bool changed = false;

			try
			{
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					//Updates the timeOUT in the database
					String query = "UPDATE residencestudent SET visitorRestricted = '" + restriction + "' WHERE stID = " + stID;
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter();
					dataAdapter.UpdateCommand = command;
					if (dataAdapter.UpdateCommand.ExecuteNonQuery() == 1)
					{
						refreshResStudents(index);
						changed = true;
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

			return changed;
		}

		private void btnChangeRestrict_Click(object sender, EventArgs e)
		{

			int index = lstStudents.SelectedIndex;
			if (index != -1)
			{
				ResidenceStudent student = listResStudents2.get(index); //Retrieves the Student object from the Generic Array List
				if (student != null)
				{
					if (student.getVisitorRestriction())
					{
						if(changeRestriction(0, student.getStID(), index))
						{
							MessageBox.Show(student.getName() + " has been Unrestricted and can now sign in visitors.");
						}
						else
						{
							MessageBox.Show("Failed to Unrestrict " + student.getName());
						}
					}
					else
					{
						if (changeRestriction(1, student.getStID(), index))
						{
							MessageBox.Show(student.getName() + " has been Restricted and can no longer sign in visitors.");
						}
						else
						{
							MessageBox.Show("Failed to Restrict " + student.getName());
						}
					}
				}
				else
				{
					MessageBox.Show("ERROR: There was a problem retrieving the student object. Please call the administrator.");
				}
			}
				
		}

		private bool removeStudent(int stID, int index)
		{
			bool removed = false;

			try
			{
				con.Close();
				con.Open();
				if (con.State == ConnectionState.Open)
				{
					//Updates the timeOUT in the database
					String query = "DELETE FROM residencestudent WHERE stID = " + stID;
					MySqlCommand command = new MySqlCommand(query, con);
					dataAdapter = new MySqlDataAdapter();
					dataAdapter.DeleteCommand = command;
					if (dataAdapter.DeleteCommand.ExecuteNonQuery() == 1)
					{
						//Deletes current object from Generic Array List
						ResidenceStudent student = listResStudents2.remove(index);
						MessageBox.Show(student.getName() + " " + student.getSurname() + " has been removed successfully.");
						removed = true;
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

			return removed;
		}

		private bool decrementAvailableStudent(int index)
		{
			bool updated = false;

			//Decrement from ArrayList
			Residence residence = listResidences.get(index);
			int availableStudents = residence.getAvailableStu();
			availableStudents--; //Decrement available students
			residence.setAvailableStudents(availableStudents);
			listResidences.update(index, residence); //Updates ArrayList with new Updated object

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

		private void btnRemoveStudent_Click(object sender, EventArgs e)
		{
			if(lstStudents.SelectedIndex != -1)
			{
				if (removeStudent(listResStudents2.get(lstStudents.SelectedIndex).getStID(), lstStudents.SelectedIndex))
				{
					//Update the number of Current Res Students
					if (decrementAvailableStudent(cmbResidences.SelectedIndex))
						refreshResStudents(cmbResidences.SelectedIndex);
					
				}
			}
			else
			{
				MessageBox.Show("Please select a Student to Remove.");
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

		private void btnAddNewStu_Click(object sender, EventArgs e)
		{
			if (listResidences.get(cmbResidences.SelectedIndex).getNumOfRooms() - listResidences.get(cmbResidences.SelectedIndex).getAvailableStu() != 0)
			{
				if (cmbResidences.SelectedIndex != -1)
				{
					if (txtAddUniID.Text != "")
					{
						int stID = getStID(txtAddUniID.Text);
						if (stID != -1)
							frmMain.openAddResStudent(this, stID, listResidences.get(cmbResidences.SelectedIndex), cmbResidences.SelectedIndex);
						else
							MessageBox.Show("Student Not Found. Please Try Again.");
					}
				}
				else
				{
					MessageBox.Show("Please select a Residence to add student.");
				}
			}
			else
			{
				MessageBox.Show("Unfortunately there aren't any rooms left at this Residence. Please select a different one.");
			}
			
			
		}

		private void sendStudentRequestEmail(string email)
		{
			//formulate the code for sending the email to request the student here
			//(NB! Use the email in the parameters - It belongs to the selected student)
		}

		private void btnRequestStudent_Click(object sender, EventArgs e)
		{
			//Call the selected index from lstVisitor (Validate if it's not equal to -1)
			int index = lstVisitors.SelectedIndex;
			if (index != -1)
			{
				//Use the index to retrieve the student object from the Generic Array List of Residence Students
				ResidenceStudent student = listResStudents.get(index);
				if (student != null)
				{
					string studentEmail = student.getEmail(); //Gets selected studen's email
					sendStudentRequestEmail(studentEmail); //Sends the Student an email about the visitor
				}
				else
				{
					MessageBox.Show("ERROR: Failed to retrieve student object. Please call administrator.");
				}
			}
			else
			{
				MessageBox.Show("Please select the Visitor to contact.");
			}
			
		}

		private void rbAsending_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAsending.Checked)
			{
				dgVisitors.Sort(dgVisitors.Columns[0], ListSortDirection.Ascending);
			}	
			else if (rbDescending.Checked)
			{
				dgVisitors.Sort(dgVisitors.Columns[0], ListSortDirection.Descending);
			}
		}

		private void cmbResVisitor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbResVisitor.SelectedIndex != -1)
			{
				if(getResIDFromArrayList(cmbResVisitor.SelectedItem.ToString()) != -1)
				{
					currentResID = getResIDFromArrayList(cmbResVisitor.SelectedItem.ToString());
					cmbResVisitor.Enabled = false;
				}
				else
				{
					MessageBox.Show("ERROR: Couldn't find resID in the ArrayList. Please Call Administrator.");
				}
				
			}
		}

		private bool printReport()
		{
			bool printed = false;

			//Make method of type string and return the file name
			//PrintDialog printDialog = new PrintDialog();
			//printDialog.Document = printDocumentNow;
			printDialogVisitor.Document = printDocumentVisitor;

			//DialogResult result = printNow.ShowDialog();
			if (printDialogVisitor.ShowDialog() == DialogResult.OK)
			{
				printDocumentVisitor.Print();
				printed = true;
			}

			return printed;
		}

		private void compileContents(string surname, string name, string date, string timeIn, string timeOut, string stName, string stSurname, string roomNum, string empName, string empSurname)
		{
			//string resStudent = stName[0] + ". " + stSurname;
			//string employee = empName[0] + ". " + empSurname;

			//Cocatenate the contents of the report
			reportContent += "\n" + String.Format("{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}", name, surname, timeIn, timeOut, stSurname, roomNum, empSurname);

		}

		private void printDocumentVisitor_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawString("Visitors Report", new Font("Arial", 40, FontStyle.Underline), Brushes.Black, new Point(200, 10));
			e.Graphics.DrawString(dateFilterVisitors.Value.ToString("yyyy-MM-dd"), new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(350, 100));
			e.Graphics.DrawString(String.Format("{0,-15} {1,-15}", "Residence: ", cmbFilterRes.SelectedItem.ToString()), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 175));
			e.Graphics.DrawString(String.Format("\n{0,-15} {1,-15} {2,-15} {3,-15} {4,-15} {5,-15} {6,-15}\n", "Name", "Surname", "Time In", "Time Out", "Res-Student", "Room No.", "Employee"), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(20, 175));
			e.Graphics.DrawString(reportContent + "\n\nNumber of Visitors: " + dgVisitors.RowCount, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(20, 175));
		}

		private void btnPrintReport_Click(object sender, EventArgs e)
		{
			int counter = 0;

			while (counter < dgVisitors.RowCount)
			{
				string name = dgVisitors.Rows[counter].Cells[0].FormattedValue.ToString();
				string surname = dgVisitors.Rows[counter].Cells[1].FormattedValue.ToString();
				string date = dgVisitors.Rows[counter].Cells[2].FormattedValue.ToString();
				string timeIn = dgVisitors.Rows[counter].Cells[3].FormattedValue.ToString();
				string timeOut = dgVisitors.Rows[counter].Cells[4].FormattedValue.ToString();
				string stName = dgVisitors.Rows[counter].Cells[5].FormattedValue.ToString();
				string stSurname = dgVisitors.Rows[counter].Cells[6].FormattedValue.ToString();
				string roomNum = dgVisitors.Rows[counter].Cells[7].FormattedValue.ToString();
				string empName = dgVisitors.Rows[counter].Cells[8].FormattedValue.ToString();
				string empSurname = dgVisitors.Rows[counter].Cells[9].FormattedValue.ToString();

				compileContents(surname, name, date, timeIn, timeOut, stName, stSurname, roomNum, empName, empSurname);
				counter++;
			}

			if(reportContent != "")
			{
				reportContent = "\n\n\n" + reportContent;
				if (printReport()) MessageBox.Show("Visitors Report printed successfully.");
			}
			else
			{
				MessageBox.Show("No Visitor Information Found. Please filter the information of visitors to print.");
			}
		}
	}
}
