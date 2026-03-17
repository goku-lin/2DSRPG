using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeProject.Data;

namespace ResumeProject.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProjectsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await db.Projects.Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            Status = p.Status.ToString(),
            p.Progress,
            p.StartDate,
            p.EndDate
        }).ToListAsync();

        return Ok(projects);
    }
}
