namespace ResumeProject.DTOs;

public class DashboardDto
{
    public string ProjectName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double CompletionRate { get; set; }
    public Dictionary<string, int> StatusDistribution { get; set; } = new();
    public List<BurnDownPointDto> Burndown { get; set; } = new();
    public List<string> RiskTasks { get; set; } = new();
}

public class BurnDownPointDto
{
    public string Day { get; set; } = string.Empty;
    public int RemainingPoints { get; set; }
}

public record CreateTaskRequest(Guid ProjectId, string Title, string Priority, int StoryPoint, DateTime Deadline, Guid AssigneeId);
public record MoveTaskRequest(string NewStatus);
