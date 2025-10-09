using COMP2139_ICE.Data;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;
[Route("ProjectTask")] //localhost:5090/ProjectTask
public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context; 
    public ProjectTaskController(ApplicationDbContext context) : base() 
    { 
        _context = context; 
    } 

    
    [HttpGet("Index/{projectId:int}")]
    public IActionResult Index(int projectId) 
    { 
        var tasks = _context.ProjectTasks 
                            .Where(t => t.ProjectId == projectId) 
                            .ToList(); 
        ViewBag.ProjectId = projectId;   

        return View(tasks); 
    } 

    
    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id) 
    { 
        var task = _context.ProjectTasks 

                        .Include(t => t.Project) // Include related project data 

                        .FirstOrDefault(t => t.ProjectTaskId == id); 

        if (task == null) 
        { 
            return NotFound(); 
        } 
        
        return View(task); 
    } 

    [HttpGet("Create")] 
    public IActionResult Create(int projectId) 
    { 
        var project = _context.Projects.Find(projectId); 

        if (project == null) 
        { 
            return NotFound(); 
        } 
        
        var task = new ProjectTask 
        { 
            ProjectId = projectId, 

            Title = "", 

            Description = "" 
        }; 
        return View(task); 
    } 

    
    [HttpPost("Create")] 
    [ValidateAntiForgeryToken] 

    public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task) 
    { 
        if (ModelState.IsValid) 
        { 
            _context.ProjectTasks.Add(task); 

            _context.SaveChanges(); 
            
            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId }); 
        } 
        
        ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId); 

        return View(task); 
    } 

    
    [HttpGet("Edit/{id:int}")] 
    public IActionResult Edit(int id) 
    { 
        var task = _context.ProjectTasks 

                            .Include(t => t.Project) // Include related project data 

                            .FirstOrDefault(t => t.ProjectTaskId == id); 

        if (task == null) 
        { 
            return NotFound(); 
        } 
        
        ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId); 

        return View(task); 
    } 
    
    
    [HttpPost("Edit/{id:int}")] 

    [ValidateAntiForgeryToken] 

    public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task) 

    { 
        
        if (id != task.ProjectTaskId) 
        { 
            return NotFound(); 
        } 

        if (ModelState.IsValid) 
        { 

            _context.ProjectTasks.Update(task); 

            _context.SaveChanges(); 

            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId }); 

        } 
        
        ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId); 

        return View(task); 
    } 

    
    [HttpGet("Delete/{id:int}")] 

    public IActionResult Delete(int id) 
    { 
        var task = _context.ProjectTasks 

                            .Include(t => t.Project) // Include related project data 

                            .FirstOrDefault(t => t.ProjectTaskId == id); 
        
        if (task == null) 
        { 
            return NotFound(); 
        } 
        
        return View(task); 
    } 

 

    [HttpPost ("DeleteConfirmed/{id:int}")] 

    [ValidateAntiForgeryToken] 

    public IActionResult DeleteConfirmed(int projectTaskId) 
    { 
        var task = _context.ProjectTasks.Find(projectTaskId); 

        if (task != null) 
        { 
            _context.ProjectTasks.Remove(task); 
            _context.SaveChanges(); 

            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });

        } 
        
        return NotFound(); 
    } 
    
    // Lab 6 - Search ProjectTasks
    // GET: ProjectTasks/Search/{projectId?}/{searchString?}
    [HttpGet("Search/{searchString?}")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        // Start with all tasks as an IQueryable query (deferred execution)
        var taskQuery = _context.ProjectTasks.AsQueryable();

        // Track whether a search was performed
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        // If a projectId is provided, filter by project
        if (projectId.HasValue)
        {
            taskQuery = taskQuery.Where(t => t.ProjectId == projectId.Value);
        }

        // ❗ FIXED: Apply search filter when searchString is provided
        if (searchPerformed)
        {
            searchString = searchString.ToLower(); // Case-insensitive search

            // Ensure null-safe search on nullable Description
            taskQuery = taskQuery.Where(t =>
                t.Title.ToLower().Contains(searchString) ||
                (t.Description != null && t.Description.ToLower().Contains(searchString))
            );
        }

        // ❗ WHY ASYNC? ❗
        // The database query is executed asynchronously using `ToListAsync()`
        // This prevents blocking the main thread while waiting for the result.
        var tasks = await taskQuery.ToListAsync();

        // Pass search metadata to the view for UI updates
        ViewBag.ProjectId = projectId;
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;

        // Reuse Index view to display filtered results
        return View("Index", tasks);
    }
}