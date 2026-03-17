using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ResumeProject.Data;
using ResumeProject.Domain;
using ResumeProject.DTOs;
using ResumeProject.Hubs;

namespace ResumeProject.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController(AppDbContext db, IHubContext<NotificationHub> hub) : ControllerBase
{
    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> ByProject(Guid projectId)
    {
        var tasks = await db.Tasks.Where(t => t.ProjectId == projectId)
            .Select(t => new
            {
                t.Id,
                t.Title,
                Status = t.Status.ToString(),
                t.Priority,
                t.StoryPoint,
                t.Deadline
            }).ToListAsync();

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Priority = request.Priority,
            StoryPoint = request.StoryPoint,
            Deadline = request.Deadline,
            AssigneeId = request.AssigneeId,
            Status = TaskStatus.Todo
        };

        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        await hub.Clients.Group(request.ProjectId.ToString()).SendAsync("TaskCreated", new { task.Id, task.Title, Status = task.Status.ToString() });

        return Ok(task);
    }

    [HttpPatch("{taskId:guid}/move")]
    public async Task<IActionResult> Move(Guid taskId, MoveTaskRequest request)
    {
        var task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null)
        {
            return NotFound();
        }

        if (!Enum.TryParse<TaskStatus>(request.NewStatus, true, out var newStatus))
        {
            return BadRequest("非法状态");
        }

        task.Status = newStatus;
        await db.SaveChangesAsync();

        await hub.Clients.Group(task.ProjectId.ToString()).SendAsync("TaskMoved", new { task.Id, Status = task.Status.ToString() });
        return Ok(task);
    }
}
