using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class RegisterForm : Form
    {
        private UserRepository userRepo;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtFullName;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblFullName;
        private Label lblPassword;
        private Label lblConfirmPassword;
        private Panel panel1;

        public RegisterForm()
        {
            userRepo = new UserRepository();
            this.StartPosition = FormStartPosition.CenterParent;
            SetupForm();
        }

        private void SetupForm()
        {
            // Form Settings
            this.Text = "Create New Account";
            this.Size = new Size(500, 600);
            this.BackColor = Color.FromArgb(52, 73, 94);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Panel
            panel1 = new Panel();
            panel1.BackColor = Color.White;
            panel1.Size = new Size(420, 450);
            panel1.Location = new Point(40, 70);
            panel1.BorderStyle = BorderStyle.FixedSingle;

            // Title Label
            lblTitle = new Label();
            lblTitle.Text = "Register New Account";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(40, 25);
            lblTitle.Size = new Size(400, 40);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Labels and TextBoxes
            int yPos = 30;
            int spacing = 45;

            // Username
            lblUsername = new Label();
            lblUsername.Text = "Username*";
            lblUsername.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(52, 73, 94);
            lblUsername.Location = new Point(40, yPos);
            lblUsername.Size = new Size(340, 20);

            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 10);
            txtUsername.Location = new Point(40, yPos + 22);
            txtUsername.Size = new Size(340, 25);
            txtUsername.BackColor = Color.FromArgb(236, 240, 241);

            // Email
            yPos += spacing;
            lblEmail = new Label();
            lblEmail.Text = "Email*";
            lblEmail.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(52, 73, 94);
            lblEmail.Location = new Point(40, yPos);
            lblEmail.Size = new Size(340, 20);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Location = new Point(40, yPos + 22);
            txtEmail.Size = new Size(340, 25);
            txtEmail.BackColor = Color.FromArgb(236, 240, 241);

            // Full Name
            yPos += spacing;
            lblFullName = new Label();
            lblFullName.Text = "Full Name*";
            lblFullName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFullName.ForeColor = Color.FromArgb(52, 73, 94);
            lblFullName.Location = new Point(40, yPos);
            lblFullName.Size = new Size(340, 20);

            txtFullName = new TextBox();
            txtFullName.Font = new Font("Segoe UI", 10);
            txtFullName.Location = new Point(40, yPos + 22);
            txtFullName.Size = new Size(340, 25);
            txtFullName.BackColor = Color.FromArgb(236, 240, 241);

            // Password
            yPos += spacing;
            lblPassword = new Label();
            lblPassword.Text = "Password* (minimum 6 characters)";
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(52, 73, 94);
            lblPassword.Location = new Point(40, yPos);
            lblPassword.Size = new Size(340, 20);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.Location = new Point(40, yPos + 22);
            txtPassword.Size = new Size(340, 25);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.BackColor = Color.FromArgb(236, 240, 241);

            // Confirm Password
            yPos += spacing;
            lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Confirm Password*";
            lblConfirmPassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblConfirmPassword.ForeColor = Color.FromArgb(52, 73, 94);
            lblConfirmPassword.Location = new Point(40, yPos);
            lblConfirmPassword.Size = new Size(340, 20);

            txtConfirmPassword = new TextBox();
            txtConfirmPassword.Font = new Font("Segoe UI", 10);
            txtConfirmPassword.Location = new Point(40, yPos + 22);
            txtConfirmPassword.Size = new Size(340, 25);
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.BackColor = Color.FromArgb(236, 240, 241);

            // Buttons
            yPos += spacing + 10;
            btnRegister = new Button();
            btnRegister.Text = "REGISTER";
            btnRegister.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnRegister.BackColor = Color.FromArgb(46, 204, 113);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Size = new Size(160, 40);
            btnRegister.Location = new Point(90, yPos);
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += new EventHandler(BtnRegister_Click);

            btnCancel = new Button();
            btnCancel.Text = "CANCEL";
            btnCancel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnCancel.BackColor = Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Size = new Size(160, 40);
            btnCancel.Location = new Point(270, yPos);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += new EventHandler(BtnCancel_Click);

            // Add controls to panel
            panel1.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblEmail, txtEmail, lblFullName, txtFullName,
                lblPassword, txtPassword, lblConfirmPassword, txtConfirmPassword,
                btnRegister, btnCancel
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] { lblTitle, panel1 });
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (txtUsername.Text.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                btnRegister.Enabled = false;
                btnRegister.Text = "Processing...";

                // Check if user exists
                bool userExists = await System.Threading.Tasks.Task.Run(() => userRepo.UserExists(txtUsername.Text.Trim()));
                if (userExists)
                {
                    MessageBox.Show("Username already exists. Please choose another.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                bool emailExists = await System.Threading.Tasks.Task.Run(() => userRepo.EmailExists(txtEmail.Text.Trim()));
                if (emailExists)
                {
                    MessageBox.Show("Email already registered. Please use another email or login.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                var registerModel = new RegisterModel
                {
                    Username = txtUsername.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text,
                    ConfirmPassword = txtConfirmPassword.Text,
                    FullName = txtFullName.Text.Trim()
                };

                bool result = await System.Threading.Tasks.Task.Run(() => userRepo.RegisterUser(registerModel));

                if (result)
                {
                    MessageBox.Show("Registration successful! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "REGISTER";
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}