using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;

namespace EFGetStartedAgain;
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

     public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Todo> Todos { get; set; }

    public DbSet<Team> Teams {get; set;}
    public DbSet<Worker>Workers {get; set;}
    public DbSet<TeamWorker> TeamWorkers {get; set;}

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TeamWorker>()
            .HasKey(p => new {p.TeamId, p.WorkerId});
            
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; } ="";

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}

public class TaskItem
{
    public int TaskItemId { get; set; }
    public string Name { get; set; } = "";

    public List<Todo> Todos { get; set; } = new();
}
public class Todo
{
    public int TodoId { get; set; }
    public string Name { get; set; } = "";
    public bool IsComplete { get; set; }

    public int TaskItemId { get; set; }          // FK
    public TaskItem TaskItem { get; set; } = null!;
}