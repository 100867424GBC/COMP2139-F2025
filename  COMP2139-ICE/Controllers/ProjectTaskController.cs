using COMP2139_ICE.Data;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;

public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context; 
    public ProjectTaskController(ApplicationDbContext context) : base() 
    { 
        _context = context; 
    } 

    
    [HttpGet] 
    public IActionResult Index(int projectId) 
    { 
        var tasks = _context.ProjectTasks 
                            .Where(t => t.ProjectId == projectId) 
                            .ToList(); 
        ViewBag.ProjectId = projectId;   

        return View(tasks); 
    } 

    
    [HttpGet] 
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

    [HttpGet] 
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

    
    [HttpPost] 
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

    
    [HttpGet] 
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
    
    
    [HttpPost] 

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

    
    [HttpGet] 

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

 

    [HttpPost, ActionName("DeleteConfirmed")] 

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
}