using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class AddProjectForm : Form
    {
        private int userId;
        private ProjectRepository projectRepo;
        private TextBox txtName;
        private TextBox txtDescription;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private ComboBox cmbStatus;
        private Button btnSave;
        private Button btnCancel;

        public AddProjectForm(int userId)
        {
            this.userId = userId;
            projectRepo = new ProjectRepository();
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Add New Project";
            this.Size = new Size(580, 580);  // Increased height
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Create New Project";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(25, 20);
            lblTitle.Size = new Size(500, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // Project Name
            Label lblName = new Label();
            lblName.Text = "Project Name *";
            lblName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(52, 73, 94);
            lblName.Location = new Point(25, 80);
            lblName.Size = new Size(150, 25);

            txtName = new TextBox();
            txtName.Font = new Font("Segoe UI", 11);
            txtName.Location = new Point(25, 110);
            txtName.Size = new Size(510, 30);
            txtName.BackColor = Color.FromArgb(236, 240, 241);
            txtName.BorderStyle = BorderStyle.FixedSingle;

            // Description
            Label lblDescription = new Label();
            lblDescription.Text = "Description";
            lblDescription.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDescription.ForeColor = Color.FromArgb(52, 73, 94);
            lblDescription.Location = new Point(25, 160);
            lblDescription.Size = new Size(150, 25);

            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 11);
            txtDescription.Location = new Point(25, 190);
            txtDescription.Size = new Size(510, 80);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.FromArgb(236, 240, 241);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;

            // Start Date
            Label lblStartDate = new Label();
            lblStartDate.Text = "Start Date";
            lblStartDate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStartDate.ForeColor = Color.FromArgb(52, 73, 94);
            lblStartDate.Location = new Point(25, 290);
            lblStartDate.Size = new Size(150, 25);

            dtpStartDate = new DateTimePicker();
            dtpStartDate.Font = new Font("Segoe UI", 11);
            dtpStartDate.Location = new Point(25, 320);
            dtpStartDate.Size = new Size(240, 30);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Value = DateTime.Now;

            // End Date
            Label lblEndDate = new Label();
            lblEndDate.Text = "End Date";
            lblEndDate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblEndDate.ForeColor = Color.FromArgb(52, 73, 94);
            lblEndDate.Location = new Point(295, 290);
            lblEndDate.Size = new Size(150, 25);

            dtpEndDate = new DateTimePicker();
            dtpEndDate.Font = new Font("Segoe UI", 11);
            dtpEndDate.Location = new Point(295, 320);
            dtpEndDate.Size = new Size(240, 30);
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Value = DateTime.Now.AddMonths(1);

            // Status
            Label lblStatus = new Label();
            lblStatus.Text = "Status";
            lblStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Location = new Point(25, 370);
            lblStatus.Size = new Size(150, 25);

            cmbStatus = new ComboBox();
            cmbStatus.Font = new Font("Segoe UI", 11);
            cmbStatus.Location = new Point(25, 400);
            cmbStatus.Size = new Size(240, 30);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "Active", "On Hold", "Completed" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.BackColor = Color.FromArgb(236, 240, 241);

            // Separator
            Panel separator = new Panel();
            separator.BackColor = Color.FromArgb(200, 200, 200);
            separator.Size = new Size(530, 1);
            separator.Location = new Point(25, 460);

            // Save Button
            btnSave = new Button();
            btnSave.Text = "✓ CREATE PROJECT";
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Size = new Size(220, 45);
            btnSave.Location = new Point(130, 490);
            btnSave.Cursor = Cursors.Hand;
            btnSave.Click += new EventHandler(BtnSave_Click);

            btnSave.MouseEnter += (s, e) => { btnSave.BackColor = Color.FromArgb(39, 174, 96); };
            btnSave.MouseLeave += (s, e) => { btnSave.BackColor = Color.FromArgb(46, 204, 113); };

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "✗ CANCEL";
            btnCancel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Size = new Size(220, 45);
            btnCancel.Location = new Point(360, 490);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += new EventHandler(BtnCancel_Click);

            btnCancel.MouseEnter += (s, e) => { btnCancel.BackColor = Color.FromArgb(192, 57, 43); };
            btnCancel.MouseLeave += (s, e) => { btnCancel.BackColor = Color.FromArgb(231, 76, 60); };

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblTitle, lblName, txtName, lblDescription, txtDescription,
                lblStartDate, dtpStartDate, lblEndDate, dtpEndDate,
                lblStatus, cmbStatus, separator, btnSave, btnCancel
            });

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a project name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (dtpEndDate.Value < dtpStartDate.Value)
            {
                MessageBox.Show("End date cannot be earlier than start date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return;
            }

            try
            {
                Project project = new Project
                {
                    UserId = userId,
                    Name = txtName.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                    StartDate = dtpStartDate.Value,
                    EndDate = dtpEndDate.Value,
                    Status = cmbStatus.SelectedItem.ToString(),
                    CreatedAt = DateTime.Now
                };

                int projectId = projectRepo.CreateProject(project);

                if (projectId > 0)
                {
                    MessageBox.Show($"Project '{project.Name}' created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create project. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating project: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel? Any entered data will be lost.",
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtName.Focus();
        }
    }
}