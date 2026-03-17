namespace ResumeProject.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Member";
}

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public int Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid OwnerId { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();
}

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public TaskStatus Status { get; set; }
    public int StoryPoint { get; set; }
    public DateTime Deadline { get; set; }
    public Guid AssigneeId { get; set; }
}

public class WorkLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public decimal Hours { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
