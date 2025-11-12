using System;
using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        [Display(Name = "Task Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Task Description")]
        public string? Description { get; set; }

        // Foreign Key
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        // Navigation property
        
        [Display(Name = "Related Project")]
        public Project? Project { get; set; }

        // Date the task was created or posted
        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    }
}