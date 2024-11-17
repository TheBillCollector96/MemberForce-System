
namespace MemberForce2._0
{
	partial class ReportMisconductingStudent
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gridStudents = new System.Windows.Forms.DataGridView();
			this.btnPrintReport = new System.Windows.Forms.Button();
			this.lblUniversityID2 = new System.Windows.Forms.Label();
			this.txtUniversityID2 = new System.Windows.Forms.TextBox();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtLocation = new System.Windows.Forms.TextBox();
			this.txtUnivID = new System.Windows.Forms.TextBox();
			this.tbViewMisconductees = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbDescending = new System.Windows.Forms.RadioButton();
			this.rbAsending = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblHelp = new System.Windows.Forms.Label();
			this.gbReportedStudents = new System.Windows.Forms.GroupBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblMisconduct = new System.Windows.Forms.Label();
			this.lblUniversityID = new System.Windows.Forms.Label();
			this.btnLogOut = new System.Windows.Forms.Button();
			this.btnReportStudents = new System.Windows.Forms.Button();
			this.gbMisconductDetails = new System.Windows.Forms.GroupBox();
			this.cmbMisc = new System.Windows.Forms.ComboBox();
			this.lblLocation = new System.Windows.Forms.Label();
			this.tbReportMisconduct = new System.Windows.Forms.TabPage();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.lblMemberName = new System.Windows.Forms.Label();
			this.printMisc = new System.Windows.Forms.PrintDialog();
			this.printReportedStud = new System.Drawing.Printing.PrintDocument();
			((System.ComponentModel.ISupportInitialize)(this.gridStudents)).BeginInit();
			this.tbViewMisconductees.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.gbReportedStudents.SuspendLayout();
			this.gbMisconductDetails.SuspendLayout();
			this.tbReportMisconduct.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridStudents
			// 
			this.gridStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridStudents.Location = new System.Drawing.Point(0, 19);
			this.gridStudents.Name = "gridStudents";
			this.gridStudents.RowHeadersWidth = 62;
			this.gridStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridStudents.Size = new System.Drawing.Size(454, 385);
			this.gridStudents.TabIndex = 0;
			this.gridStudents.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridStudents_CellDoubleClick);
			// 
			// btnPrintReport
			// 
			this.btnPrintReport.Location = new System.Drawing.Point(512, 348);
			this.btnPrintReport.Margin = new System.Windows.Forms.Padding(2);
			this.btnPrintReport.Name = "btnPrintReport";
			this.btnPrintReport.Size = new System.Drawing.Size(166, 46);
			this.btnPrintReport.TabIndex = 5;
			this.btnPrintReport.Text = "Print Report";
			this.btnPrintReport.UseVisualStyleBackColor = true;
			this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
			// 
			// lblUniversityID2
			// 
			this.lblUniversityID2.AutoSize = true;
			this.lblUniversityID2.Location = new System.Drawing.Point(565, 163);
			this.lblUniversityID2.Name = "lblUniversityID2";
			this.lblUniversityID2.Size = new System.Drawing.Size(73, 13);
			this.lblUniversityID2.TabIndex = 3;
			this.lblUniversityID2.Text = "University ID: ";
			// 
			// txtUniversityID2
			// 
			this.txtUniversityID2.Location = new System.Drawing.Point(494, 190);
			this.txtUniversityID2.Name = "txtUniversityID2";
			this.txtUniversityID2.Size = new System.Drawing.Size(200, 20);
			this.txtUniversityID2.TabIndex = 2;
			this.txtUniversityID2.TextChanged += new System.EventHandler(this.txtUniversityID2_TextChanged);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(494, 94);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker1.TabIndex = 1;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(143, 116);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(364, 246);
			this.txtDescription.TabIndex = 7;
			// 
			// txtLocation
			// 
			this.txtLocation.Location = new System.Drawing.Point(143, 90);
			this.txtLocation.Name = "txtLocation";
			this.txtLocation.Size = new System.Drawing.Size(364, 20);
			this.txtLocation.TabIndex = 6;
			// 
			// txtUnivID
			// 
			this.txtUnivID.Location = new System.Drawing.Point(143, 35);
			this.txtUnivID.Name = "txtUnivID";
			this.txtUnivID.Size = new System.Drawing.Size(364, 20);
			this.txtUnivID.TabIndex = 4;
			// 
			// tbViewMisconductees
			// 
			this.tbViewMisconductees.Controls.Add(this.groupBox2);
			this.tbViewMisconductees.Controls.Add(this.label2);
			this.tbViewMisconductees.Controls.Add(this.label1);
			this.tbViewMisconductees.Controls.Add(this.lblHelp);
			this.tbViewMisconductees.Controls.Add(this.btnPrintReport);
			this.tbViewMisconductees.Controls.Add(this.lblUniversityID2);
			this.tbViewMisconductees.Controls.Add(this.txtUniversityID2);
			this.tbViewMisconductees.Controls.Add(this.dateTimePicker1);
			this.tbViewMisconductees.Controls.Add(this.gbReportedStudents);
			this.tbViewMisconductees.Location = new System.Drawing.Point(4, 22);
			this.tbViewMisconductees.Name = "tbViewMisconductees";
			this.tbViewMisconductees.Padding = new System.Windows.Forms.Padding(3);
			this.tbViewMisconductees.Size = new System.Drawing.Size(738, 422);
			this.tbViewMisconductees.TabIndex = 1;
			this.tbViewMisconductees.Text = "View Misconducting Students";
			this.tbViewMisconductees.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbDescending);
			this.groupBox2.Controls.Add(this.rbAsending);
			this.groupBox2.Location = new System.Drawing.Point(533, 229);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(126, 100);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sort Permits";
			// 
			// rbDescending
			// 
			this.rbDescending.AutoSize = true;
			this.rbDescending.Location = new System.Drawing.Point(10, 60);
			this.rbDescending.Name = "rbDescending";
			this.rbDescending.Size = new System.Drawing.Size(82, 17);
			this.rbDescending.TabIndex = 1;
			this.rbDescending.TabStop = true;
			this.rbDescending.Text = "Descending";
			this.rbDescending.UseVisualStyleBackColor = true;
			// 
			// rbAsending
			// 
			this.rbAsending.AutoSize = true;
			this.rbAsending.Location = new System.Drawing.Point(10, 29);
			this.rbAsending.Name = "rbAsending";
			this.rbAsending.Size = new System.Drawing.Size(75, 17);
			this.rbAsending.TabIndex = 0;
			this.rbAsending.TabStop = true;
			this.rbAsending.Text = "Ascending";
			this.rbAsending.UseVisualStyleBackColor = true;
			this.rbAsending.CheckedChanged += new System.EventHandler(this.rbAsending_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label2.Location = new System.Drawing.Point(509, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(169, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "On Student Record to View Profile";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label1.Location = new System.Drawing.Point(509, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "and University ID then Double Click ";
			// 
			// lblHelp
			// 
			this.lblHelp.AutoSize = true;
			this.lblHelp.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.lblHelp.Location = new System.Drawing.Point(509, 25);
			this.lblHelp.Name = "lblHelp";
			this.lblHelp.Size = new System.Drawing.Size(194, 13);
			this.lblHelp.TabIndex = 9;
			this.lblHelp.Text = "Filter Reported Students using the date ";
			// 
			// gbReportedStudents
			// 
			this.gbReportedStudents.Controls.Add(this.gridStudents);
			this.gbReportedStudents.Location = new System.Drawing.Point(6, 6);
			this.gbReportedStudents.Name = "gbReportedStudents";
			this.gbReportedStudents.Size = new System.Drawing.Size(465, 410);
			this.gbReportedStudents.TabIndex = 0;
			this.gbReportedStudents.TabStop = false;
			this.gbReportedStudents.Text = "Reported Students";
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(6, 130);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(66, 13);
			this.lblDescription.TabIndex = 3;
			this.lblDescription.Text = "Description: ";
			// 
			// lblMisconduct
			// 
			this.lblMisconduct.AutoSize = true;
			this.lblMisconduct.Location = new System.Drawing.Point(6, 71);
			this.lblMisconduct.Name = "lblMisconduct";
			this.lblMisconduct.Size = new System.Drawing.Size(68, 13);
			this.lblMisconduct.TabIndex = 1;
			this.lblMisconduct.Text = "Misconduct: ";
			// 
			// lblUniversityID
			// 
			this.lblUniversityID.AutoSize = true;
			this.lblUniversityID.Location = new System.Drawing.Point(6, 42);
			this.lblUniversityID.Name = "lblUniversityID";
			this.lblUniversityID.Size = new System.Drawing.Size(73, 13);
			this.lblUniversityID.TabIndex = 0;
			this.lblUniversityID.Text = "University ID: ";
			// 
			// btnLogOut
			// 
			this.btnLogOut.Location = new System.Drawing.Point(593, 550);
			this.btnLogOut.Name = "btnLogOut";
			this.btnLogOut.Size = new System.Drawing.Size(145, 36);
			this.btnLogOut.TabIndex = 3;
			this.btnLogOut.Text = "Log Out";
			this.btnLogOut.UseVisualStyleBackColor = true;
			this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
			// 
			// btnReportStudents
			// 
			this.btnReportStudents.Location = new System.Drawing.Point(540, 136);
			this.btnReportStudents.Name = "btnReportStudents";
			this.btnReportStudents.Size = new System.Drawing.Size(168, 54);
			this.btnReportStudents.TabIndex = 1;
			this.btnReportStudents.Text = "Report Students";
			this.btnReportStudents.UseVisualStyleBackColor = true;
			this.btnReportStudents.Click += new System.EventHandler(this.btnReportStudents_Click);
			// 
			// gbMisconductDetails
			// 
			this.gbMisconductDetails.Controls.Add(this.cmbMisc);
			this.gbMisconductDetails.Controls.Add(this.txtDescription);
			this.gbMisconductDetails.Controls.Add(this.txtLocation);
			this.gbMisconductDetails.Controls.Add(this.txtUnivID);
			this.gbMisconductDetails.Controls.Add(this.lblDescription);
			this.gbMisconductDetails.Controls.Add(this.lblLocation);
			this.gbMisconductDetails.Controls.Add(this.lblMisconduct);
			this.gbMisconductDetails.Controls.Add(this.lblUniversityID);
			this.gbMisconductDetails.Location = new System.Drawing.Point(6, 6);
			this.gbMisconductDetails.Name = "gbMisconductDetails";
			this.gbMisconductDetails.Size = new System.Drawing.Size(513, 397);
			this.gbMisconductDetails.TabIndex = 0;
			this.gbMisconductDetails.TabStop = false;
			this.gbMisconductDetails.Text = "Details of Misconduct";
			// 
			// cmbMisc
			// 
			this.cmbMisc.FormattingEnabled = true;
			this.cmbMisc.Items.AddRange(new object[] {
            "Cheating",
            "Assualt",
            "Threats of Violence",
            "Bullying",
            "Unauthorised consumption, possession or distribution of alcoholic beverages",
            "Possession, use or distribution of any illegal substance",
            "Unauthorized entry into, or use of, University facilities",
            "Plagiarism",
            "Vandilism",
            "Sexual Assualt",
            "Theft",
            "Malicious Destruction",
            "Damage or Injury to Property",
            "Unacceptable Behaviour or Language",
            "Other"});
			this.cmbMisc.Location = new System.Drawing.Point(143, 62);
			this.cmbMisc.Name = "cmbMisc";
			this.cmbMisc.Size = new System.Drawing.Size(364, 21);
			this.cmbMisc.TabIndex = 8;
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.lblLocation.Location = new System.Drawing.Point(6, 99);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(54, 13);
			this.lblLocation.TabIndex = 2;
			this.lblLocation.Text = "Location: ";
			// 
			// tbReportMisconduct
			// 
			this.tbReportMisconduct.Controls.Add(this.btnReportStudents);
			this.tbReportMisconduct.Controls.Add(this.gbMisconductDetails);
			this.tbReportMisconduct.Location = new System.Drawing.Point(4, 22);
			this.tbReportMisconduct.Name = "tbReportMisconduct";
			this.tbReportMisconduct.Padding = new System.Windows.Forms.Padding(3);
			this.tbReportMisconduct.Size = new System.Drawing.Size(738, 422);
			this.tbReportMisconduct.TabIndex = 0;
			this.tbReportMisconduct.Text = "ReportMisconduct";
			this.tbReportMisconduct.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tbReportMisconduct);
			this.tabControl1.Controls.Add(this.tbViewMisconductees);
			this.tabControl1.Location = new System.Drawing.Point(12, 85);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(746, 448);
			this.tabControl1.TabIndex = 1;
			// 
			// lblMemberName
			// 
			this.lblMemberName.AutoSize = true;
			this.lblMemberName.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMemberName.Location = new System.Drawing.Point(51, 25);
			this.lblMemberName.Name = "lblMemberName";
			this.lblMemberName.Size = new System.Drawing.Size(0, 39);
			this.lblMemberName.TabIndex = 13;
			// 
			// printMisc
			// 
			this.printMisc.UseEXDialog = true;
			// 
			// printReportedStud
			// 
			this.printReportedStud.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printReportedStud_PrintPage);
			// 
			// ReportMisconductingStudent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(785, 598);
			this.Controls.Add(this.lblMemberName);
			this.Controls.Add(this.btnLogOut);
			this.Controls.Add(this.tabControl1);
			this.Name = "ReportMisconductingStudent";
			this.Text = "Report Misconducting Student";
			((System.ComponentModel.ISupportInitialize)(this.gridStudents)).EndInit();
			this.tbViewMisconductees.ResumeLayout(false);
			this.tbViewMisconductees.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.gbReportedStudents.ResumeLayout(false);
			this.gbMisconductDetails.ResumeLayout(false);
			this.gbMisconductDetails.PerformLayout();
			this.tbReportMisconduct.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridStudents;
		private System.Windows.Forms.Button btnPrintReport;
		private System.Windows.Forms.Label lblUniversityID2;
		private System.Windows.Forms.TextBox txtUniversityID2;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.TextBox txtUnivID;
		private System.Windows.Forms.TabPage tbViewMisconductees;
		private System.Windows.Forms.GroupBox gbReportedStudents;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblMisconduct;
		private System.Windows.Forms.Label lblUniversityID;
		private System.Windows.Forms.Button btnLogOut;
		private System.Windows.Forms.Button btnReportStudents;
		private System.Windows.Forms.GroupBox gbMisconductDetails;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.TabPage tbReportMisconduct;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Label lblMemberName;
		private System.Windows.Forms.ComboBox cmbMisc;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblHelp;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbDescending;
		private System.Windows.Forms.RadioButton rbAsending;
		private System.Windows.Forms.PrintDialog printMisc;
		private System.Drawing.Printing.PrintDocument printReportedStud;
	}
}