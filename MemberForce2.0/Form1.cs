using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemberForce2._0
{
    public partial class MemberForce : Form
    {
        
        public MemberForce()
        {
            InitializeComponent();
        }

        private void reportMisconductingStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openEmployeeAuthentication("ReportMisc");
        }

        public void displayMainContents()
        {
            btnPermit.Visible = true;
            btnReport.Visible = true;
            btnResVisitor.Visible = true;
            picLogo.Visible = true;
            lblCredits.Visible = true;
        }

        //Open Requested forms

        //Residence Visitor
        public void openResidenceVisitor(int empID)
		{
            StudentResidenceVisitor frmResVisitor = new StudentResidenceVisitor(this, empID);//Parent Form
            frmResVisitor.MdiParent = this;
            frmResVisitor.Show();
		}

        public void openEditVisitor(StudentResidenceVisitor frmResVisit, Visitor visitor, int empID, int stID, int index)
        {
            Edit_Visitor frmEditVisitor = new Edit_Visitor(frmResVisit, visitor, empID, stID, index);//Child Form
            frmEditVisitor.MdiParent = this;
            frmEditVisitor.Show();
        }

        public void openAddResStudent(StudentResidenceVisitor frmResVisit, int stID, Residence residence, int index)
		{
            Add_New_Student frmAddResStu = new Add_New_Student(frmResVisit, stID, residence, index);
            frmAddResStu.MdiParent = this;
            frmAddResStu.Show();
        }

        //Report Misconducting Student
        public void openMisconductingStudent(int empID)
		{
            ReportMisconductingStudent frmMisconductStu = new ReportMisconductingStudent(this, empID);
            frmMisconductStu.MdiParent = this;
            frmMisconductStu.Show();
		}

        public void openStudentProfile(int stID)
        {
            StudentProfile frmProfile = new StudentProfile(stID);
            frmProfile.MdiParent = this;
            frmProfile.Show();
        }

        //Temporary Permit Generator
        public void openPermitGenerator(int empID)
		{
            TemporaryPermitGenerator frmPermit = new TemporaryPermitGenerator(this, empID);
            frmPermit.MdiParent = this;
            frmPermit.Show();
		}

        private void openEmployeeAuthentication(string frmOpen)
		{
            EmployeeAuthentication frmEmpAuth = new EmployeeAuthentication(this, frmOpen);
            frmEmpAuth.MdiParent = this;
            frmEmpAuth.Show();
            btnPermit.Visible = false;
            btnReport.Visible = false;
            btnResVisitor.Visible = false;
            picLogo.Visible = false;
            lblCredits.Visible = false;
        }

		private void studentResidenceVisitorToolStripMenuItem_Click(object sender, EventArgs e)
		{
            openEmployeeAuthentication("ResVisitor");
		}

		private void btnResVisitor_Click(object sender, EventArgs e)
		{
            openEmployeeAuthentication("ResVisitor");
        }

		private void temporaryPermitGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
            openEmployeeAuthentication("PermitGen");
        }

		private void btnPermit_Click(object sender, EventArgs e)
		{
            openEmployeeAuthentication("PermitGen");
        }

		private void btnReport_Click(object sender, EventArgs e)
		{
            openEmployeeAuthentication("ReportMisc");
        }
	}
}
