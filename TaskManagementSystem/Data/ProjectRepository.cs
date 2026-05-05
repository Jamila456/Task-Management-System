using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using TaskManagementSystem.Models;
using TaskManagementSystem.Helpers;

namespace TaskManagementSystem.Data
{
    public class ProjectRepository
    {
        public int CreateProject(Project project)
        {
            string query = @"INSERT INTO Projects (UserId, Name, Description, StartDate, EndDate, Status, CreatedAt) 
                            VALUES (@UserId, @Name, @Description, @StartDate, @EndDate, @Status, @CreatedAt);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@UserId", project.UserId),
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", (object)project.Description ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)project.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)project.EndDate ?? DBNull.Value),
                new SqlParameter("@Status", project.Status),
                new SqlParameter("@CreatedAt", project.CreatedAt)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool UpdateProject(Project project)
        {
            string query = @"UPDATE Projects SET Name = @Name, Description = @Description, 
                            StartDate = @StartDate, EndDate = @EndDate, Status = @Status 
                            WHERE Id = @Id AND UserId = @UserId";

            SqlParameter[] parameters = {
                new SqlParameter("@Id", project.Id),
                new SqlParameter("@UserId", project.UserId),
                new SqlParameter("@Name", project.Name),
                new SqlParameter("@Description", (object)project.Description ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)project.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)project.EndDate ?? DBNull.Value),
                new SqlParameter("@Status", project.Status)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public bool DeleteProject(int projectId, int userId)
        {
            string query = "DELETE FROM Projects WHERE Id = @Id AND UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@Id", projectId),
                new SqlParameter("@UserId", userId)
            };
            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public List<Project> GetProjectsByUser(int userId)
        {
            List<Project> projects = new List<Project>();
            string query = "SELECT Id, UserId, Name, Description, StartDate, EndDate, Status, CreatedAt FROM Projects WHERE UserId = @UserId ORDER BY CreatedAt DESC";

            SqlParameter[] parameters = { new SqlParameter("@UserId", userId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                Project project = new Project
                {
                    Id = Convert.ToInt32(row["Id"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    StartDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : (DateTime?)null,
                    EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : (DateTime?)null,
                    Status = row["Status"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                };

                // Load tasks for this project
                TaskRepository taskRepo = new TaskRepository();
                var tasks = taskRepo.GetTasksByProject(project.Id);
                project.Tasks = tasks; // FIXED: Assign List<TaskItem> to ICollection<TaskItem>
                projects.Add(project);
            }

            return projects;
        }

        public Project GetProjectById(int projectId, int userId)
        {
            string query = "SELECT Id, UserId, Name, Description, StartDate, EndDate, Status, CreatedAt FROM Projects WHERE Id = @Id AND UserId = @UserId";

            SqlParameter[] parameters = {
                new SqlParameter("@Id", projectId),
                new SqlParameter("@UserId", userId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Project project = new Project
                {
                    Id = Convert.ToInt32(row["Id"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    StartDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]) : (DateTime?)null,
                    EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : (DateTime?)null,
                    Status = row["Status"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                };

                TaskRepository taskRepo = new TaskRepository();
                var tasks = taskRepo.GetTasksByProject(project.Id);
                project.Tasks = tasks; // FIXED: Assign List<TaskItem> to ICollection<TaskItem>
                return project;
            }

            return null;
        }
    }
}