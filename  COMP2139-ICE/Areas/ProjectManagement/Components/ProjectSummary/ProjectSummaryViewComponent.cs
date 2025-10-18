//Week 10 - Lab 8
//file name - ProjectSummaryViewComponent.cs
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Components.ProjectSummary;

public class ProjectSummaryViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        // Query the Projects table asynchronously to retrieve the project with the specified ID.
        // Includes the related 'Tasks' navigation property to load associated tasks with the project.
        var project = await _context.Projects
            .Include(p => p.Tasks) // Load related tasks for the project (eager loading).
            .FirstOrDefaultAsync(p => p.ProjectId == projectId); // Retrieve the project with the matching ID.

        // Check if the project is null (not found in the database).
        if (project == null)
        {
            // Return a plain text response indicating the project was not found.
            return Content("Project not found");
        }

        // Return the default view for this ViewComponent, passing the project as the model.
        return View(project);
    }

}

//--Key takeaway--
//Task - Represents asynchronous execution
//<IViewComponentResult> - The type of result the task will produce
//Task<IViewComponentResult> - “An async method that eventually returns a renderable component result”
//await - Used to asynchronously wait for the completion of the task
//async - Indicates that the method can perform asynchronous operations

//DbContext - Represents a session with the database, allowing querying and saving data
//FirstOrDefaultAsync - Asynchronously retrieves the first element of a sequence or a default value
//Include - Eagerly loads related entities (in this case, tasks associated with the project)
//Content - Returns a content result with the specified string
//View - Returns a view result with the specified model (in this case, the project)
//InvokeAsync - The method that gets called when the ViewComponent is invoked, allowing for asynchronous execution and returning a result that can be rendered in a view.
//ProjectSummaryViewComponent - A ViewComponent that retrieves and displays project summary information
//ApplicationDbContext - The database context used to interact with the database, typically containing DbSet