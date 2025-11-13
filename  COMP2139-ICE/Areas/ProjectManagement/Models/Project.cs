using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class Project

{

    /// <summary>
    /// 
    /// The unique identifier for the project.
    /// 
    /// </summary>
    /// 
    public int ProjectId { get; set; }
    
    
    /// <summary>
    /// 
    /// The name of the project.
    /// 
    /// - [Required]: Ensures this property must have a value when the object is validated.
    /// 
    /// - [required]: A C# 11 feature that enforces initialization during object creation.
    /// 
    /// </summary>
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    
    /// <summary>
    /// 
    /// An optional description of the project.
    /// 
    /// - Nullable: Allows this property to have a null value.
    /// 
    /// </summary>
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    
    /// <summary>
    /// 
    /// The start date of the project.
    /// 
    /// - [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).
    /// 
    /// </summary>
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }
    
    
    /// <summary>
    /// 
    /// The end date of the project.
    /// 
    /// - [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).
    /// 
    /// </summary>
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime EndDate { get; set; }

    
    /// <summary>
    /// 
    /// The current status of the project (e.g., "In Progress," "Completed").
    /// 
    /// - Nullable: Allows this property to have a null value if the status is unknown.
    /// 
    /// </summary>
    [Display(Name = "Project Status")]
    public string? Status { get; set; }

    // One-to-Many: A Project can have many ProjectTasks
    public List<ProjectTask>? Tasks { get; set; } = new();
    
    
    // One-to-Many: A Project can have many ProjectComments
    public List<ProjectComment>? Comments { get; set; } = new();
    
}
