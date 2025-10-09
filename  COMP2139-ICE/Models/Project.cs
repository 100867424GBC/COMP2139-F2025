using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Models;

public class Project

{

    /// <summary>

    /// The unique identifier for the project.

    /// </summary>
    
    public int ProjectId { get; set; }
    
    
    /// <summary>

    /// The name of the project.

    /// - [Required]: Ensures this property must have a value when the object is validated.

    /// - [required]: A C# 11 feature that enforces initialization during object creation.

    /// </summary>
    
    [Required]
    public required string Name { get; set; }
    

    /// <summary>

    /// An optional description of the project.

    /// - Nullable: Allows this property to have a null value.

    /// </summary>
    
    public string? Description { get; set; }


    /// <summary>

    /// The start date of the project.

    /// - [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).

    /// </summary>
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }


    /// <summary>

    /// The end date of the project.

    /// - [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).

    /// </summary>
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }


    /// <summary>

    /// The current status of the project (e.g., "In Progress," "Completed").

    /// - Nullable: Allows this property to have a null value if the status is unknown.

    /// </summary>
    
    public string? Status { get; set; }
    
    // Week 6
    public List<ProjectTask>? Tasks { get; set; } = new();
    
}