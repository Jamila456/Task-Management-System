using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class AddTaskForm : Form
    {
        private int projectId;
        private TaskRepository taskRepo;
        private TextBox txtTitle;
        private TextBox txtDescription;
        private ComboBox cmbPriority;
        private ComboBox cmbStatus;
        private DateTimePicker dtpDueDate;
        private Button btnSave;
        private Button btnCancel;

        public AddTaskForm(int projectId)
        {
            this.projectId = projectId;
            taskRepo = new TaskRepository();
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            SetupForm();
        }

        private void SetupForm()
        {
            // INCREASED FORM SIZE TO SHOW ALL BUTTONS
            this.Text = "Add New Task";
            this.Size = new Size(550, 580);  // Increased height from 450 to 580
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Create New Task";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(25, 20);
            lblTitle.Size = new Size(500, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // Task Title Label
            Label lblTaskTitle = new Label();
            lblTaskTitle.Text = "Task Title *";
            lblTaskTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTaskTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTaskTitle.Location = new Point(25, 80);
            lblTaskTitle.Size = new Size(150, 25);

            // Task Title TextBox
            txtTitle = new TextBox();
            txtTitle.Font = new Font("Segoe UI", 11);
            txtTitle.Location = new Point(25, 110);
            txtTitle.Size = new Size(480, 30);
            txtTitle.BackColor = Color.FromArgb(236, 240, 241);
            txtTitle.BorderStyle = BorderStyle.FixedSingle;

            // Description Label
            Label lblDescription = new Label();
            lblDescription.Text = "Description";
            lblDescription.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDescription.ForeColor = Color.FromArgb(52, 73, 94);
            lblDescription.Location = new Point(25, 160);
            lblDescription.Size = new Size(150, 25);

            // Description TextBox
            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 11);
            txtDescription.Location = new Point(25, 190);
            txtDescription.Size = new Size(480, 80);
            txtDescription.Multiline = true;
            txtDescription.BackColor = Color.FromArgb(236, 240, 241);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
           

            // Priority Label
            Label lblPriority = new Label();
            lblPriority.Text = "Priority";
            lblPriority.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblPriority.ForeColor = Color.FromArgb(52, 73, 94);
            lblPriority.Location = new Point(25, 290);
            lblPriority.Size = new Size(150, 25);

            // Priority ComboBox
            cmbPriority = new ComboBox();
            cmbPriority.Font = new Font("Segoe UI", 11);
            cmbPriority.Location = new Point(25, 320);
            cmbPriority.Size = new Size(220, 30);
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.Items.AddRange(new object[] { "Low", "Medium", "High" });
            cmbPriority.SelectedIndex = 1;  // Default to Medium
            cmbPriority.BackColor = Color.FromArgb(236, 240, 241);

            // Priority Colors - Visual feedback
            cmbPriority.DrawMode = DrawMode.OwnerDrawFixed;
            cmbPriority.DrawItem += (sender, e) => {
                e.DrawBackground();
                string text = cmbPriority.Items[e.Index].ToString();
                Brush brush = Brushes.Black;
                if (text == "High") brush = Brushes.Red;
                else if (text == "Medium") brush = Brushes.Orange;
                else if (text == "Low") brush = Brushes.Green;
                e.Graphics.DrawString(text, cmbPriority.Font, brush, e.Bounds);
                e.DrawFocusRectangle();
            };

            // Status Label
            Label lblStatus = new Label();
            lblStatus.Text = "Status";
            lblStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Location = new Point(285, 290);
            lblStatus.Size = new Size(150, 25);

            // Status ComboBox
            cmbStatus = new ComboBox();
            cmbStatus.Font = new Font("Segoe UI", 11);
            cmbStatus.Location = new Point(285, 320);
            cmbStatus.Size = new Size(220, 30);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed" });
            cmbStatus.SelectedIndex = 0;  // Default to Pending
            cmbStatus.BackColor = Color.FromArgb(236, 240, 241);

            // Due Date Label
            Label lblDueDate = new Label();
            lblDueDate.Text = "Due Date";
            lblDueDate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDueDate.ForeColor = Color.FromArgb(52, 73, 94);
            lblDueDate.Location = new Point(25, 370);
            lblDueDate.Size = new Size(150, 25);

            // Due Date Picker
            dtpDueDate = new DateTimePicker();
            dtpDueDate.Font = new Font("Segoe UI", 11);
            dtpDueDate.Location = new Point(25, 400);
            dtpDueDate.Size = new Size(220, 30);
            dtpDueDate.Format = DateTimePickerFormat.Short;
            dtpDueDate.MinDate = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddDays(7);  // Default to 7 days from now
            dtpDueDate.BackColor = Color.FromArgb(236, 240, 241);

            // Optional: Add a checkbox for "No Due Date"
            CheckBox chkNoDueDate = new CheckBox();
            chkNoDueDate.Text = "No due date";
            chkNoDueDate.Font = new Font("Segoe UI", 10);
            chkNoDueDate.Location = new Point(285, 400);
            chkNoDueDate.Size = new Size(120, 30);
            chkNoDueDate.CheckedChanged += (s, e) => {
                dtpDueDate.Enabled = !chkNoDueDate.Checked;
                if (chkNoDueDate.Checked)
                {
                    dtpDueDate.Value = DateTime.Now;
                }
            };

            // Separator Line
            Panel separator = new Panel();
            separator.BackColor = Color.FromArgb(200, 200, 200);
            separator.Size = new Size(500, 1);
            separator.Location = new Point(25, 450);

            // SAVE Button (CREATE TASK)
            btnSave = new Button();
            btnSave.Text = "✓ CREATE TASK";
            btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Size = new Size(200, 45);
            btnSave.Location = new Point(130, 480);  // Moved down to be visible
            btnSave.Cursor = Cursors.Hand;
            btnSave.Click += new EventHandler(BtnSave_Click);

            // Mouse hover effects for Save button
            btnSave.MouseEnter += (s, e) => { btnSave.BackColor = Color.FromArgb(39, 174, 96); };
            btnSave.MouseLeave += (s, e) => { btnSave.BackColor = Color.FromArgb(46, 204, 113); };

            // CANCEL Button
            btnCancel = new Button();
            btnCancel.Text = "✗ CANCEL";
            btnCancel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Size = new Size(200, 45);
            btnCancel.Location = new Point(350, 480);  // Moved down to be visible
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Mouse hover effects for Cancel button
            btnCancel.MouseEnter += (s, e) => { btnCancel.BackColor = Color.FromArgb(192, 57, 43); };
            btnCancel.MouseLeave += (s, e) => { btnCancel.BackColor = Color.FromArgb(231, 76, 60); };

            // Add all controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblTaskTitle, txtTitle,
                lblDescription, txtDescription,
                lblPriority, cmbPriority,
                lblStatus, cmbStatus,
                lblDueDate, dtpDueDate, chkNoDueDate,
                separator,
                btnSave, btnCancel
            });

            // Set Accept Button (Enter key triggers Save)
            this.AcceptButton = btnSave;

            // Set Cancel Button (Esc key triggers Cancel)
            this.CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a task title.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (txtTitle.Text.Length > 200)
            {
                MessageBox.Show("Task title cannot exceed 200 characters.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            try
            {
                // Determine due date (null if "No due date" is checked)
                DateTime? dueDate = null;
                if (dtpDueDate.Enabled)
                {
                    dueDate = dtpDueDate.Value;
                }

                TaskItem task = new TaskItem
                {
                    ProjectId = projectId,
                    Title = txtTitle.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                    Priority = cmbPriority.SelectedItem.ToString(),
                    Status = cmbStatus.SelectedItem.ToString(),
                    DueDate = dueDate,
                    CreatedAt = DateTime.Now
                };

                int taskId = taskRepo.CreateTask(task);

                if (taskId > 0)
                {
                    MessageBox.Show($"Task '{task.Title}' created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create task. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating task: {ex.Message}", "Error",
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

        // Optional: Add form load event to set focus
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtTitle.Focus();
        }
    }
}