using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Forms
{
    public partial class ProjectDetailsForm : Form
    {
        private Project project;
        private User currentUser;
        private TaskRepository taskRepo;
        private FlowLayoutPanel tasksPanel;
        private Button btnAddTask;
        private Button btnBack;
        private Label lblProjectName;
        private Label lblProjectStatus;
        private ProgressBar progressBar;
        private Label lblProgress;

        public ProjectDetailsForm(Project project, User currentUser)
        {
            this.project = project;
            this.currentUser = currentUser;
            taskRepo = new TaskRepository();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 700);
            SetupForm();
            LoadTasks();
        }

        private void SetupForm()
        {
            this.Text = $"Project: {project.Name}";
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.BackColor = Color.FromArgb(52, 73, 94);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 120;

            lblProjectName = new Label();
            lblProjectName.Text = project.Name;
            lblProjectName.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblProjectName.ForeColor = Color.White;
            lblProjectName.Location = new Point(20, 15);
            lblProjectName.AutoSize = true;

            lblProjectStatus = new Label();
            lblProjectStatus.Text = project.Status.ToUpper();
            lblProjectStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblProjectStatus.ForeColor = Color.White;
            lblProjectStatus.BackColor = project.Status == "Active" ? Color.FromArgb(46, 204, 113) : (project.Status == "Completed" ? Color.FromArgb(52, 152, 219) : Color.FromArgb(241, 196, 15));
            lblProjectStatus.Size = new Size(80, 30);
            lblProjectStatus.Location = new Point(20, 55);
            lblProjectStatus.TextAlign = ContentAlignment.MiddleCenter;

            // Description
            if (!string.IsNullOrEmpty(project.Description))
            {
                Label lblDesc = new Label();
                lblDesc.Text = project.Description;
                lblDesc.Font = new Font("Segoe UI", 10);
                lblDesc.ForeColor = Color.FromArgb(236, 240, 241);
                lblDesc.Location = new Point(20, 95);
                lblDesc.Size = new Size(500, 20);
                headerPanel.Controls.Add(lblDesc);
            }

            // Progress Bar
            progressBar = new ProgressBar();
            progressBar.Value = project.Progress;
            progressBar.Size = new Size(250, 10);
            progressBar.Location = new Point(600, 25);

            lblProgress = new Label();
            lblProgress.Text = $"Progress: {project.Progress}%";
            lblProgress.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblProgress.ForeColor = Color.White;
            lblProgress.Location = new Point(600, 45);
            lblProgress.Size = new Size(150, 25);

            headerPanel.Controls.AddRange(new Control[] { lblProjectName, lblProjectStatus, progressBar, lblProgress });

            // Button Panel
            Panel buttonPanel = new Panel();
            buttonPanel.BackColor = Color.White;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 50;

            btnAddTask = new Button();
            btnAddTask.Text = "+ ADD NEW TASK";
            btnAddTask.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddTask.BackColor = Color.FromArgb(46, 204, 113);
            btnAddTask.ForeColor = Color.White;
            btnAddTask.FlatStyle = FlatStyle.Flat;
            btnAddTask.Size = new Size(150, 35);
            btnAddTask.Location = new Point(20, 8);
            btnAddTask.Cursor = Cursors.Hand;
            btnAddTask.Click += new EventHandler(BtnAddTask_Click);

            btnBack = new Button();
            btnBack.Text = "← BACK";
            btnBack.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnBack.BackColor = Color.FromArgb(149, 165, 166);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Size = new Size(100, 35);
            btnBack.Location = new Point(770, 8);
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += new EventHandler(BtnBack_Click);

            buttonPanel.Controls.AddRange(new Control[] { btnAddTask, btnBack });

            // Tasks Panel
            tasksPanel = new FlowLayoutPanel();
            tasksPanel.Dock = DockStyle.Fill;
            tasksPanel.AutoScroll = true;
            tasksPanel.Padding = new Padding(10);
            tasksPanel.BackColor = Color.FromArgb(236, 240, 241);

            // Add controls
            this.Controls.Add(tasksPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(headerPanel);
        }

        private void LoadTasks()
        {
            tasksPanel.Controls.Clear();

            // Refresh tasks from database
            var tasks = taskRepo.GetTasksByProject(project.Id);
            project.Tasks = tasks;

            if (tasks == null || tasks.Count == 0)
            {
                Label lblNoTasks = new Label();
                lblNoTasks.Text = "No tasks yet. Click 'Add New Task' to get started!";
                lblNoTasks.Font = new Font("Segoe UI", 12);
                lblNoTasks.ForeColor = Color.Gray;
                lblNoTasks.TextAlign = ContentAlignment.MiddleCenter;
                lblNoTasks.Size = new Size(800, 100);
                tasksPanel.Controls.Add(lblNoTasks);
                return;
            }

            // Group tasks by status
            var pendingTasks = tasks.Where(t => t.Status == "Pending").ToList();
            var inProgressTasks = tasks.Where(t => t.Status == "InProgress").ToList();
            var completedTasks = tasks.Where(t => t.Status == "Completed").ToList();

            if (pendingTasks.Any())
            {
                tasksPanel.Controls.Add(CreateTaskSection("Pending Tasks", pendingTasks, Color.FromArgb(241, 196, 15)));
            }

            if (inProgressTasks.Any())
            {
                tasksPanel.Controls.Add(CreateTaskSection("In Progress Tasks", inProgressTasks, Color.FromArgb(52, 152, 219)));
            }

            if (completedTasks.Any())
            {
                tasksPanel.Controls.Add(CreateTaskSection("Completed Tasks", completedTasks, Color.FromArgb(46, 204, 113)));
            }
        }

        private Panel CreateTaskSection(string sectionTitle, System.Collections.Generic.List<TaskItem> tasksList, Color color)
        {
            Panel sectionPanel = new Panel();
            sectionPanel.Size = new Size(850, 50 + (tasksList.Count * 100));
            sectionPanel.Margin = new Padding(0, 10, 0, 10);
            sectionPanel.BackColor = Color.White;

            Label lblTitle = new Label();
            lblTitle.Text = sectionTitle;
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = color;
            lblTitle.Location = new Point(15, 10);
            lblTitle.Size = new Size(300, 30);

            sectionPanel.Controls.Add(lblTitle);

            int yPos = 50;
            foreach (var task in tasksList)
            {
                sectionPanel.Controls.Add(CreateTaskCard(task, yPos));
                yPos += 100;
            }

            return sectionPanel;
        }

        private Panel CreateTaskCard(TaskItem task, int yPosition)
        {
            Panel card = new Panel();
            card.Size = new Size(820, 90);
            card.Location = new Point(15, yPosition);
            card.BackColor = Color.FromArgb(248, 249, 250);
            card.BorderStyle = BorderStyle.FixedSingle;

            // Task Title
            Label lblTitle = new Label();
            lblTitle.Text = task.Title;
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblTitle.Location = new Point(15, 10);
            lblTitle.Size = new Size(300, 25);

            // Priority Badge
            Label lblPriority = new Label();
            lblPriority.Text = task.Priority.ToUpper();
            lblPriority.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblPriority.ForeColor = Color.White;
            lblPriority.BackColor = task.Priority == "High" ? Color.FromArgb(231, 76, 60) : (task.Priority == "Medium" ? Color.FromArgb(241, 196, 15) : Color.FromArgb(46, 204, 113));
            lblPriority.Size = new Size(60, 20);
            lblPriority.Location = new Point(330, 10);
            lblPriority.TextAlign = ContentAlignment.MiddleCenter;

            // Due Date
            if (task.DueDate.HasValue)
            {
                Label lblDueDate = new Label();
                lblDueDate.Text = $"Due: {task.DueDate.Value.ToString("MMM dd, yyyy")}";
                lblDueDate.Font = new Font("Segoe UI", 9);
                lblDueDate.ForeColor = (task.DueDate.Value.Date < DateTime.Now.Date && task.Status != "Completed") ? Color.FromArgb(231, 76, 60) : Color.Gray;
                lblDueDate.Location = new Point(15, 40);
                lblDueDate.Size = new Size(150, 20);
                card.Controls.Add(lblDueDate);
            }

            // Description (if exists)
            if (!string.IsNullOrEmpty(task.Description))
            {
                Label lblDesc = new Label();
                lblDesc.Text = task.Description.Length > 50 ? task.Description.Substring(0, 47) + "..." : task.Description;
                lblDesc.Font = new Font("Segoe UI", 9);
                lblDesc.ForeColor = Color.Gray;
                lblDesc.Location = new Point(15, 65);
                lblDesc.Size = new Size(500, 20);
                card.Controls.Add(lblDesc);
            }

            // Status ComboBox
            ComboBox cmbStatus = new ComboBox();
            cmbStatus.Items.AddRange(new object[] { "Pending", "InProgress", "Completed" });
            cmbStatus.SelectedItem = task.Status;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Size = new Size(120, 25);
            cmbStatus.Location = new Point(500, 30);
            cmbStatus.Tag = task;
            cmbStatus.SelectedIndexChanged += (s, e) => {
                var combo = s as ComboBox;
                var selectedTask = combo.Tag as TaskItem;
                if (selectedTask != null)
                {
                    selectedTask.Status = combo.SelectedItem.ToString();
                    taskRepo.UpdateTask(selectedTask);

                    // Refresh progress
                    var allTasks = taskRepo.GetTasksByProject(project.Id);
                    int completedCount = allTasks.Count(t => t.Status == "Completed");
                    int totalTasks = allTasks.Count;
                    int newProgress = totalTasks > 0 ? (completedCount * 100) / totalTasks : 0;

                    progressBar.Value = newProgress;
                    lblProgress.Text = $"Progress: {newProgress}%";

                    LoadTasks(); // Refresh the view
                }
            };

            // Delete Button
            Button btnDelete = new Button();
            btnDelete.Text = "🗑️ Delete";
            btnDelete.Font = new Font("Segoe UI", 9);
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Size = new Size(80, 30);
            btnDelete.Location = new Point(640, 30);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Tag = task;
            btnDelete.Click += (s, e) => {
                var btn = s as Button;
                var taskToDelete = btn.Tag as TaskItem;
                if (taskToDelete != null)
                {
                    DialogResult result = MessageBox.Show($"Delete task '{taskToDelete.Title}'?", "Confirm Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        taskRepo.DeleteTask(taskToDelete.Id);
                        LoadTasks();

                        // Update progress
                        var allTasks = taskRepo.GetTasksByProject(project.Id);
                        int newCompletedCount = allTasks.Count(t => t.Status == "Completed");
                        int newTotalTasks = allTasks.Count;
                        int newProgress = newTotalTasks > 0 ? (newCompletedCount * 100) / newTotalTasks : 0;
                        progressBar.Value = newProgress;
                        lblProgress.Text = $"Progress: {newProgress}%";
                    }
                }
            };

            card.Controls.AddRange(new Control[] { lblTitle, lblPriority, cmbStatus, btnDelete });

            return card;
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            AddTaskForm addTaskForm = new AddTaskForm(project.Id);
            if (addTaskForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh project tasks
                var tasks = taskRepo.GetTasksByProject(project.Id);
                project.Tasks = tasks;
                LoadTasks();

                // Update progress
                int completedCount = tasks.Count(t => t.Status == "Completed");
                int totalTasks = tasks.Count;
                int newProgress = totalTasks > 0 ? (completedCount * 100) / totalTasks : 0;
                progressBar.Value = newProgress;
                lblProgress.Text = $"Progress: {newProgress}%";
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}