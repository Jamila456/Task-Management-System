using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    // Use TaskItem to avoid conflict with System.Threading.Tasks.Task
    public class TaskItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Priority { get; set; } = "Medium"; // Low, Medium, High
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed

        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual ICollection<TaskComment> Comments { get; set; }

        // Calculated property
        public bool IsOverdue
        {
            get
            {
                if (DueDate.HasValue && Status != "Completed")
                    return DueDate.Value.Date < DateTime.Now.Date;
                return false;
            }
        }
    }
}