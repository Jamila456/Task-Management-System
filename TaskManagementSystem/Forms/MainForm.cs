using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class MainForm : Form
    {
        private User currentUser;
        private ProjectRepository projectRepo;
        private Panel sidebarPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private FlowLayoutPanel projectsPanel;
        private Button btnDashboard;
        private Button btnProjects;
        private Button btnAddProject;
        private Button btnLogout;
        private Label lblWelcome;

        public MainForm()
        {
            currentUser = LoginForm.LoggedInUser;
            projectRepo = new ProjectRepository();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            SetupForm();
            LoadProjects();
        }

        private void SetupForm()
        {
            this.Text = "Task Management System - Dashboard";
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header Panel
            headerPanel = new Panel();
            headerPanel.BackColor = Color.FromArgb(52, 73, 94);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;

            lblWelcome = new Label();
            lblWelcome.Text = $"Welcome, {currentUser.FullName}!";
            lblWelcome.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(20, 15);
            lblWelcome.AutoSize = true;

            btnLogout = new Button();
            btnLogout.Text = "LOGOUT";
            btnLogout.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogout.BackColor = Color.FromArgb(231, 76, 60);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Size = new Size(100, 35);
            btnLogout.Location = new Point(this.Width - 120, 12);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += new EventHandler(BtnLogout_Click);

            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnLogout);

            // Sidebar Panel
            sidebarPanel = new Panel();
            sidebarPanel.BackColor = Color.FromArgb(44, 62, 80);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 220;

            btnDashboard = CreateSidebarButton("📊 DASHBOARD", 10);
            btnProjects = CreateSidebarButton("📁 MY PROJECTS", 70);
            btnAddProject = CreateSidebarButton("➕ ADD PROJECT", 130);

            sidebarPanel.Controls.AddRange(new Control[] { btnDashboard, btnProjects, btnAddProject });

            // Content Panel
            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(236, 240, 241);
            contentPanel.Padding = new Padding(20);

            // Projects Panel (FlowLayout)
            projectsPanel = new FlowLayoutPanel();
            projectsPanel.Dock = DockStyle.Fill;
            projectsPanel.AutoScroll = true;
            projectsPanel.WrapContents = true;
            projectsPanel.Padding = new Padding(10);

            contentPanel.Controls.Add(projectsPanel);

            // Add controls to form
            this.Controls.Add(contentPanel);
            this.Controls.Add(sidebarPanel);
            this.Controls.Add(headerPanel);

            // Event handlers
            btnDashboard.Click += new EventHandler(BtnDashboard_Click);
            btnProjects.Click += new EventHandler(BtnProjects_Click);
            btnAddProject.Click += new EventHandler(BtnAddProject_Click);

            // Load dashboard by default
            BtnDashboard_Click(null, null);
        }

        private Button CreateSidebarButton(string text, int yPosition)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(44, 62, 80);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(220, 50);
            btn.Location = new Point(0, yPosition);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => { btn.BackColor = Color.FromArgb(52, 73, 94); };
            btn.MouseLeave += (s, e) => { btn.BackColor = Color.FromArgb(44, 62, 80); };

            return btn;
        }

        private void LoadProjects()
        {
            projectsPanel.Controls.Clear();

            var projects = projectRepo.GetProjectsByUser(currentUser.Id);

            if (projects.Count == 0)
            {
                Label lblNoProjects = new Label();
                lblNoProjects.Text = "No projects yet. Click 'Add Project' to create your first project!";
                lblNoProjects.Font = new Font("Segoe UI", 14);
                lblNoProjects.ForeColor = Color.Gray;
                lblNoProjects.TextAlign = ContentAlignment.MiddleCenter;
                lblNoProjects.Dock = DockStyle.Fill;
                projectsPanel.Controls.Add(lblNoProjects);
                return;
            }

            foreach (var project in projects)
            {
                projectsPanel.Controls.Add(CreateProjectCard(project));
            }
        }

        private Panel CreateProjectCard(Project project)
        {
            Panel card = new Panel();
            card.Size = new Size(350, 200);
            card.BackColor = Color.White;
            card.Margin = new Padding(10);
            card.Cursor = Cursors.Hand;
            card.Tag = project;

            // Add shadow effect
            card.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid);
            };

            // Project Name
            Label lblName = new Label();
            lblName.Text = project.Name;
            lblName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(52, 73, 94);
            lblName.Location = new Point(15, 15);
            lblName.Size = new Size(320, 30);

            // Project Status Badge
            Label lblStatus = new Label();
            lblStatus.Text = project.Status.ToUpper();
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.BackColor = project.Status == "Active" ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);
            lblStatus.Size = new Size(70, 25);
            lblStatus.Location = new Point(265, 15);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // Progress Bar
            ProgressBar progressBar = new ProgressBar();
            progressBar.Value = project.Progress;
            progressBar.Size = new Size(320, 10);
            progressBar.Location = new Point(15, 60);
            progressBar.BackColor = Color.FromArgb(236, 240, 241);

            // Progress Label
            Label lblProgress = new Label();
            lblProgress.Text = $"Progress: {project.Progress}%";
            lblProgress.Font = new Font("Segoe UI", 9);
            lblProgress.ForeColor = Color.Gray;
            lblProgress.Location = new Point(15, 75);
            lblProgress.Size = new Size(150, 20);

            // Task Count
            int totalTasks = project.Tasks?.Count ?? 0;
            int completedTasks = 0;
            if (project.Tasks != null)
            {
                completedTasks = project.Tasks.Count(t => t.Status == "Completed");
            }
            Label lblTasks = new Label();
            lblTasks.Text = $"📋 Tasks: {completedTasks}/{totalTasks} completed";
            lblTasks.Font = new Font("Segoe UI", 9);
            lblTasks.ForeColor = Color.Gray;
            lblTasks.Location = new Point(15, 100);
            lblTasks.Size = new Size(200, 20);

            // Due Date
            if (project.EndDate.HasValue)
            {
                Label lblDueDate = new Label();
                lblDueDate.Text = $"📅 Due: {project.EndDate.Value.ToString("MMM dd, yyyy")}";
                lblDueDate.Font = new Font("Segoe UI", 9);
                lblDueDate.ForeColor = project.EndDate.Value < DateTime.Now ? Color.FromArgb(231, 76, 60) : Color.Gray;
                lblDueDate.Location = new Point(15, 125);
                lblDueDate.Size = new Size(200, 20);
                card.Controls.Add(lblDueDate);
            }

            // View Details Button
            Button btnView = new Button();
            btnView.Text = "View Details →";
            btnView.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnView.BackColor = Color.FromArgb(52, 152, 219);
            btnView.ForeColor = Color.White;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.Size = new Size(120, 35);
            btnView.Location = new Point(215, 150);
            btnView.Cursor = Cursors.Hand;
            btnView.Click += (s, e) => {
                ProjectDetailsForm detailsForm = new ProjectDetailsForm(project, currentUser);
                detailsForm.ShowDialog();
                LoadProjects(); // Refresh
            };

            card.Controls.AddRange(new Control[] { lblName, lblStatus, progressBar, lblProgress, lblTasks, btnView });

            return card;
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            LoadProjects();
            HighlightButton(btnDashboard);
        }

        private void BtnProjects_Click(object sender, EventArgs e)
        {
            LoadProjects();
            HighlightButton(btnProjects);
        }

        private void BtnAddProject_Click(object sender, EventArgs e)
        {
            AddProjectForm addForm = new AddProjectForm(currentUser.Id);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadProjects();
            }
            HighlightButton(btnAddProject);
        }

        private void HighlightButton(Button activeButton)
        {
            foreach (Button btn in new Button[] { btnDashboard, btnProjects, btnAddProject })
            {
                btn.BackColor = Color.FromArgb(44, 62, 80);
                btn.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            }
            activeButton.BackColor = Color.FromArgb(52, 73, 94);
            activeButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (btnLogout != null)
            {
                btnLogout.Location = new Point(this.ClientSize.Width - 120, 12);
            }
        }
    }
}