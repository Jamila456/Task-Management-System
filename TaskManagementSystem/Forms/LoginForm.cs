using System;
using System.Drawing;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        private UserRepository userRepo;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private Panel panel1;

        public static User LoggedInUser { get; private set; }

        public LoginForm()
        {
            userRepo = new UserRepository();
            this.StartPosition = FormStartPosition.CenterScreen;
            SetupForm();
        }

        private void SetupForm()
        {
            // Form Settings
            this.Text = "Task Management System - Login";
            this.Size = new Size(450, 500);
            this.BackColor = Color.FromArgb(52, 73, 94);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Panel
            panel1 = new Panel();
            panel1.BackColor = Color.White;
            panel1.Size = new Size(380, 350);
            panel1.Location = new Point(35, 75);
            panel1.BorderStyle = BorderStyle.FixedSingle;

            // Title Label
            lblTitle = new Label();
            lblTitle.Text = "Task Manager Pro";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(50, 20);
            lblTitle.Size = new Size(350, 50);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 11);
            lblUsername.ForeColor = Color.FromArgb(52, 73, 94);
            lblUsername.Location = new Point(50, 50);
            lblUsername.Size = new Size(280, 25);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Font = new Font("Segoe UI", 11);
            txtUsername.Location = new Point(50, 80);
            txtUsername.Size = new Size(280, 30);
            txtUsername.BackColor = Color.FromArgb(236, 240, 241);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 11);
            lblPassword.ForeColor = Color.FromArgb(52, 73, 94);
            lblPassword.Location = new Point(50, 130);
            lblPassword.Size = new Size(280, 25);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.Location = new Point(50, 160);
            txtPassword.Size = new Size(280, 30);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.BackColor = Color.FromArgb(236, 240, 241);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "LOGIN";
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(46, 204, 113);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Size = new Size(280, 40);
            btnLogin.Location = new Point(50, 220);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += new EventHandler(BtnLogin_Click);

            // Register Button
            btnRegister = new Button();
            btnRegister.Text = "Create New Account";
            btnRegister.Font = new Font("Segoe UI", 10);
            btnRegister.BackColor = Color.FromArgb(52, 73, 94);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Size = new Size(280, 35);
            btnRegister.Location = new Point(50, 275);
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += new EventHandler(BtnRegister_Click);

            // Add controls to panel
            panel1.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblPassword, txtPassword, btnLogin, btnRegister
            });

            // Add controls to form
            this.Controls.AddRange(new Control[] { lblTitle, panel1 });

            // Enter key event
            this.AcceptButton = btnLogin;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Logging in...";

                var user = await System.Threading.Tasks.Task.Run(() =>
                    userRepo.AuthenticateUser(txtUsername.Text.Trim(), txtPassword.Text));

                if (user != null)
                {
                    LoggedInUser = user;
                    MessageBox.Show($"Welcome back, {user.FullName}!", "Login Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "LOGIN";
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}