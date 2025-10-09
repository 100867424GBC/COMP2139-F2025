using COMP2139_ICE.Data;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;
[Route("Project")] //localhost:5090/Project
public class ProjectController : Controller

{
    // *_context; 
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    // *GET: Project
    [HttpGet("")]
    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }

    // GET: Project/Create
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    // *POST: Project/Create
    [HttpPost("Create")]
    
    [ValidateAntiForgeryToken]

    public IActionResult Create(Project project)

    {
        if (ModelState.IsValid)
        {
            // Convert to UTC before saving
            project.StartDate = ToUtc(project.StartDate);
            project.EndDate = ToUtc(project.EndDate);
            
            _context.Projects.Add(project);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        return View(project);
    }
    
    private DateTime ToUtc(DateTime input)

    {
        if (input.Kind == DateTimeKind.Utc)
            return input;

        if (input.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(input, DateTimeKind.Utc); // assume local is already UTC

        return input.ToUniversalTime();
    }

    // *GET: Project/Details/5
    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id)
    {
        // FirstOrDefault used to find first project wth matching project ID
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    // GET: Project/Edit/5
    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    // POST: Project/Edit/5
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectId,Name,Description")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Projects.Update(project);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    // GET: Project/Delete/5
    [HttpGet("Delete/{id:int}")]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = _context.Projects
            .FirstOrDefault(p => p.ProjectId == id);

        if (project == null)
        {
            return NotFound();
        }

        return View(project); // Loads Delete.cshtml
    }
    
    // POST: Project/Delete/5
    [HttpPost("DeleteConfirmed/{id:int}")]

    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        return View(project);
    }

    // Helper method for concurrency checks
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }
    
    
    
    // Lab 6 - Project Search Functionality
    // Custom route for search functionality
    // Accessible at /Projects/Search/{searchString?}
    [HttpGet("Search/{searchString?}")]
    public async Task<IActionResult> Search(string searchString)
    {
        // Fetch all projects from the database as an IQueryable collection
        // IQueryable allows us to apply filters before executing the database query
        var projectsQuery = _context.Projects.AsQueryable();

        // Check if a search string was provided (avoids null or empty search issues)
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (searchPerformed)
        {
            // Convert searchString to lowercase to make the search case-insensitive
            searchString = searchString.ToLower();

            // Apply filtering: Match project name or description
            // Description is checked for null before calling ToLower() to prevent NullReferenceException
            projectsQuery = projectsQuery.Where(p =>
                p.Name.ToLower().Contains(searchString) ||
                (p.Description != null && p.Description.ToLower().Contains(searchString)));
        }

        // ❗ WHY ASYNC? ❗
        // Asynchronous execution means this method does not block the thread while waiting for the database.
        // Instead of blocking, ASP.NET Core can process other incoming requests while waiting for the result.
        // This improves scalability and application responsiveness.
        
        // Execute the query asynchronously using `ToListAsync()`
        var projects = await projectsQuery.ToListAsync();
        
        // ❗ HOW ASYNC WORKS HERE? ❗
        // `await` releases the current thread while waiting for the query execution to complete.
        // When the database call finishes, execution resumes on this method at this point.

        // Store search metadata for the view
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;

        // Return the filtered list to the Index view (reusing existing UI)
        return View("Index", projects);
    }
}
