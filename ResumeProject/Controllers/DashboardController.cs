using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeProject.Services;

namespace ResumeProject.Controllers;

[ApiController]
[Authorize]
[Route("api/projects/{projectId:guid}/dashboard")]
public class DashboardController(DashboardService dashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var data = await dashboardService.BuildAsync(projectId);
        return Ok(data);
    }
}
