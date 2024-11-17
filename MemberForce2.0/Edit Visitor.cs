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
    public partial class Edit_Visitor : Form
    {
        //Establish connection to the database
        //SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MemberForce_db.mdf;Integrated Security=True");
        MySqlConnection con = new MySqlConnection(@"Server=memberforce-db.ckvdhochyzki.eu-north-1.rds.amazonaws.com;UserID=root;Password=password;Database=memberforce");
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;

        private StudentResidenceVisitor frmResVisit;
        private Visitor visitor;
        private int empID;
        private int stID;
        private int index;

        public Edit_Visitor(StudentResidenceVisitor frmResVisit, Visitor visitor, int empID, int stID, int index)
        {
            InitializeComponent();
            this.frmResVisit = frmResVisit;
            this.index = index;
            this.visitor = visitor;
            this.empID = empID;
            this.stID = stID;

            //Fill Textboxes with visitor's details to be edited
            txtEditName.Text = visitor.getName();
            txtEditSurname.Text = visitor.getSurname();
            txtEditNationalID.Text = visitor.getNationalID();
            txtEditPhone.Text = visitor.getPhone();
            txtEditEmail.Text = visitor.getEmail();
        }

        private bool updateDatabase()
        {
            bool updated = false;
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    String query = "UPDATE visitors SET nationalID = '" + txtEditNationalID.Text + "', name = '" + txtEditName.Text + "', surname = '" + txtEditSurname.Text 
                        + "', phone = '" + txtEditPhone.Text + "', email = '" + txtEditEmail.Text + "' WHERE visitorID = " + visitor.getVisitorID();
                    MySqlCommand command = new MySqlCommand(query, con);
                    dataAdapter = new MySqlDataAdapter();
                    dataAdapter.UpdateCommand = command;
                    if (dataAdapter.UpdateCommand.ExecuteNonQuery() == 1)
                    {
                        updated = true;
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

        //Update Visitor Object
        private bool updateVisitorObject()
        {
            visitor.setName(txtEditName.Text);
            visitor.setSurname(txtEditSurname.Text);
            visitor.setNationalID(txtEditNationalID.Text);
            visitor.setPhone(txtEditPhone.Text);
            visitor.setEmail(txtEditEmail.Text);

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(txtEditName.Text != "")
            {
                if(txtEditSurname.Text != "")
                {
                    if(txtEditNationalID.Text != "")
                    {
                        if(txtEditPhone.Text != "")
                        {
                            if(txtEditEmail.Text != "")
                            {
                                bool dbUpdated = updateDatabase();
                                if (dbUpdated)
                                {
                                    bool objUpdated = updateVisitorObject();
                                    if (objUpdated)
                                    {
                                        frmResVisit.updateVisitorList(visitor, index);
                                        MessageBox.Show("Visitor Details Updated Successfully.");
                                        this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("ERROR: Visitor Object Update Failed. Please call administrator.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("ERROR: Failed to update database. Please call administrator.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please enter the visitor's Email.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter the visitor's Phone.");
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
    }
}
