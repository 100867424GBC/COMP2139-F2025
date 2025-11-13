using COMP2139_ICE.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace COMP2139_ICE.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<ProjectComment> ProjectComments { get; set; }
    
    //Week6
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Define One-to-Many Relationship: One Project has Many ProjectTasks
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks) // One Project has many ProjectTasks
            .WithOne(t => t.Project) // Each ProjectTask belongs to one Project
            .HasForeignKey(t => t.ProjectId) // Foreign key in ProjectTask table
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete ProjectTasks when a Project is deleted

        // Define One-to-Many Relationship: One Project has Many ProjectComments
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Comments) // One Project has many comments
            .WithOne(c => c.Project)   // Each comment belongs to one Project
            .HasForeignKey(c => c.ProjectId) // Foreign key in ProjectComment table
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete comments when a Project is deleted

        // Seeding Projects
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Assignment 1", Description = "COMP2139 Assignment 1" },
            new Project { ProjectId = 2, Name = "Assignment 2", Description = "COMP2139 Assignment 2" }
        );
    }
    
}
