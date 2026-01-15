using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFGetStartedAgain;
using var db = new BloggingContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create


if(!db.Tasks.Any())
{
    var task1 = new TaskItem
    {
        Name = "Rengøring",
        Todos =
        {
            new Todo {Name = "Støvsug", IsComplete = false},
            new Todo {Name = "Vask gulv", IsComplete = true},
            new Todo {Name = "Tør støv af", IsComplete = false},
        }
    };

    var task2 = new TaskItem
    {
        Name = ""
    };
};

Console.WriteLine("Inserting a new blog");
db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
await db.SaveChangesAsync();

// Read
Console.WriteLine("Querying for a blog");
var blog = await db.Blogs
    .OrderBy(b => b.BlogId)
    .FirstAsync();

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
blog.Posts.Add(
    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
await db.SaveChangesAsync();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
await db.SaveChangesAsync();