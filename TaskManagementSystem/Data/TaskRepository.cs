using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using TaskManagementSystem.Models;
using TaskManagementSystem.Helpers;

namespace TaskManagementSystem.Data
{
    public class TaskRepository
    {
        public int CreateTask(TaskItem task)
        {
            string query = @"INSERT INTO Tasks (ProjectId, Title, Description, Priority, Status, DueDate, CreatedAt) 
                            VALUES (@ProjectId, @Title, @Description, @Priority, @Status, @DueDate, @CreatedAt);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@ProjectId", task.ProjectId),
                new SqlParameter("@Title", task.Title),
                new SqlParameter("@Description", (object)task.Description ?? DBNull.Value),
                new SqlParameter("@Priority", task.Priority),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@DueDate", (object)task.DueDate ?? DBNull.Value),
                new SqlParameter("@CreatedAt", task.CreatedAt)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool UpdateTask(TaskItem task)
        {
            string query = @"UPDATE Tasks SET Title = @Title, Description = @Description, 
                            Priority = @Priority, Status = @Status, DueDate = @DueDate,
                            CompletedAt = @CompletedAt
                            WHERE Id = @Id";

            // If status is completed and CompletedAt is null, set it to now
            DateTime? completedAt = task.CompletedAt;
            if (task.Status == "Completed" && completedAt == null)
                completedAt = DateTime.Now;
            else if (task.Status != "Completed")
                completedAt = null;

            SqlParameter[] parameters = {
                new SqlParameter("@Id", task.Id),
                new SqlParameter("@Title", task.Title),
                new SqlParameter("@Description", (object)task.Description ?? DBNull.Value),
                new SqlParameter("@Priority", task.Priority),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@DueDate", (object)task.DueDate ?? DBNull.Value),
                new SqlParameter("@CompletedAt", (object)completedAt ?? DBNull.Value)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public bool DeleteTask(int taskId)
        {
            string query = "DELETE FROM Tasks WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", taskId) };
            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public List<TaskItem> GetTasksByProject(int projectId)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            string query = "SELECT Id, ProjectId, Title, Description, Priority, Status, DueDate, CreatedAt, CompletedAt FROM Tasks WHERE ProjectId = @ProjectId ORDER BY CASE Priority WHEN 'High' THEN 1 WHEN 'Medium' THEN 2 WHEN 'Low' THEN 3 END, DueDate ASC";

            SqlParameter[] parameters = { new SqlParameter("@ProjectId", projectId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                TaskItem task = new TaskItem
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ProjectId = Convert.ToInt32(row["ProjectId"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Priority = row["Priority"].ToString(),
                    Status = row["Status"].ToString(),
                    DueDate = row["DueDate"] != DBNull.Value ? Convert.ToDateTime(row["DueDate"]) : (DateTime?)null,
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    CompletedAt = row["CompletedAt"] != DBNull.Value ? Convert.ToDateTime(row["CompletedAt"]) : (DateTime?)null
                };
                tasks.Add(task);
            }

            return tasks;
        }

        public TaskItem GetTaskById(int taskId)
        {
            string query = "SELECT Id, ProjectId, Title, Description, Priority, Status, DueDate, CreatedAt, CompletedAt FROM Tasks WHERE Id = @Id";

            SqlParameter[] parameters = { new SqlParameter("@Id", taskId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new TaskItem
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ProjectId = Convert.ToInt32(row["ProjectId"]),
                    Title = row["Title"].ToString(),
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                    Priority = row["Priority"].ToString(),
                    Status = row["Status"].ToString(),
                    DueDate = row["DueDate"] != DBNull.Value ? Convert.ToDateTime(row["DueDate"]) : (DateTime?)null,
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    CompletedAt = row["CompletedAt"] != DBNull.Value ? Convert.ToDateTime(row["CompletedAt"]) : (DateTime?)null
                };
            }

            return null;
        }

        public int GetTaskCountByStatus(int projectId, string status)
        {
            string query = "SELECT COUNT(*) FROM Tasks WHERE ProjectId = @ProjectId AND Status = @Status";
            SqlParameter[] parameters = {
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@Status", status)
            };
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }
    }
}