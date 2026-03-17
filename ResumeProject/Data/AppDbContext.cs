using Microsoft.EntityFrameworkCore;
using ResumeProject.Domain;

namespace ResumeProject.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<WorkLog> WorkLogs => Set<WorkLog>();
}
