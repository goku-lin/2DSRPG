using ResumeProject.Data;
using ResumeProject.Domain;

namespace ResumeProject.Services;

public class SeedDataService(AppDbContext db)
{
    public Task SeedAsync()
    {
        if (db.Users.Any())
        {
            return Task.CompletedTask;
        }

        var admin = new User { UserName = "admin", Password = "Admin@123", Role = "Admin" };
        var lead = new User { UserName = "lead", Password = "Lead@123", Role = "Lead" };
        var dev = new User { UserName = "dev", Password = "Dev@123", Role = "Member" };

        var project = new Project
        {
            Name = "智能仓储调度平台",
            Description = "支持任务编排、工单追踪、燃尽图分析和实时通知",
            Status = ProjectStatus.InProgress,
            Progress = 58,
            StartDate = DateTime.UtcNow.AddDays(-15),
            EndDate = DateTime.UtcNow.AddDays(30),
            OwnerId = lead.Id
        };

        var tasks = new List<TaskItem>
        {
            new()
            {
                ProjectId = project.Id,
                Title = "对接WMS库存API",
                Priority = "High",
                Status = TaskStatus.Doing,
                StoryPoint = 8,
                Deadline = DateTime.UtcNow.AddDays(4),
                AssigneeId = dev.Id
            },
            new()
            {
                ProjectId = project.Id,
                Title = "拣货路径优化算法",
                Priority = "High",
                Status = TaskStatus.Review,
                StoryPoint = 13,
                Deadline = DateTime.UtcNow.AddDays(6),
                AssigneeId = lead.Id
            },
            new()
            {
                ProjectId = project.Id,
                Title = "异常预警面板",
                Priority = "Medium",
                Status = TaskStatus.Todo,
                StoryPoint = 5,
                Deadline = DateTime.UtcNow.AddDays(10),
                AssigneeId = dev.Id
            }
        };

        db.Users.AddRange(admin, lead, dev);
        db.Projects.Add(project);
        db.Tasks.AddRange(tasks);
        db.WorkLogs.Add(new WorkLog { TaskId = tasks[0].Id, UserId = dev.Id, Hours = 4.5m });
        db.WorkLogs.Add(new WorkLog { TaskId = tasks[1].Id, UserId = lead.Id, Hours = 3m });

        return db.SaveChangesAsync();
    }
}
