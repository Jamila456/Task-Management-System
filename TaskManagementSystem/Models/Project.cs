using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties - FIXED: Use ICollection<TaskItem> instead of ICollection<Task>
        public virtual User User { get; set; }
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        // Calculated property
        public int Progress
        {
            get
            {
                if (Tasks == null || Tasks.Count == 0) return 0;
                int completedTasks = 0;
                foreach (var task in Tasks)
                {
                    if (task.Status == "Completed")
                        completedTasks++;
                }
                return (completedTasks * 100) / Tasks.Count;
            }
        }
    }
}