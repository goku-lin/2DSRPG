using Microsoft.EntityFrameworkCore;
using ResumeProject.Data;
using ResumeProject.Domain;
using ResumeProject.DTOs;

namespace ResumeProject.Services;

public class DashboardService(AppDbContext db)
{
    public async Task<DashboardDto> BuildAsync(Guid projectId)
    {
        var project = await db.Projects.FirstAsync(p => p.Id == projectId);
        var tasks = await db.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();

        var statusCount = tasks
            .GroupBy(t => t.Status.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var totalPoints = tasks.Sum(t => t.StoryPoint);
        var donePoints = tasks.Where(t => t.Status == TaskStatus.Done).Sum(t => t.StoryPoint);
        var completionRate = totalPoints == 0 ? 0 : Math.Round((double)donePoints / totalPoints * 100, 2);

        var burndown = Enumerable.Range(0, 7)
            .Select(day => new BurnDownPointDto
            {
                Day = DateTime.UtcNow.Date.AddDays(-6 + day).ToString("MM-dd"),
                RemainingPoints = Math.Max(totalPoints - (day * 3), 0)
            })
            .ToList();

        return new DashboardDto
        {
            ProjectName = project.Name,
            Status = project.Status.ToString(),
            CompletionRate = completionRate,
            StatusDistribution = statusCount,
            Burndown = burndown,
            RiskTasks = tasks.Where(t => t.Deadline < DateTime.UtcNow.AddDays(3) && t.Status != TaskStatus.Done)
                             .Select(t => t.Title)
                             .ToList()
        };
    }
}
