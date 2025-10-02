using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required] // asp.net on the web page
    public required string Title { get; set; }
    // check that data in the feild
    
    [Required]
    public required string Description { get; set; }
    
    public int ProjectId { get; set; }
    
    public Project? Project { get; set; }
    
}