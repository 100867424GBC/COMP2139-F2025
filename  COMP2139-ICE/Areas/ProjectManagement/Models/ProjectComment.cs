using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace COMP2139_ICE.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int ProjectCommentId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; }

        // Foreign key to Project
        [Required]
        public int ProjectId { get; set; }

        // Navigation property should NOT be bound from JSON
        [JsonIgnore]
        public Project Project { get; set; }
    }
}