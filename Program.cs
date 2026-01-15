using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFGetStartedAgain;


// Note: This sample requires the database to be created before running.
//Console.WriteLine($"Database path: {db.DbPath}.");

public class Program
{
    public static void Main()
    {
        using var db = new BloggingContext();

        // Sørger for at databasen oprettes/er opdateret
        db.Database.Migrate();

        // Seed data
        SeedTasks(db);

        // Test-print (valgfrit)
        var tasks = db.Tasks.Include(t => t.Todos).ToList();
        foreach (var task in tasks)
        {
            Console.WriteLine($"Task: {task.Name}");
            foreach (var todo in task.Todos)
            {
                Console.WriteLine($"  - {(todo.IsComplete ? "[x]" : "[ ]")} {todo.Name}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("DB path: " + db.DbPath);
    }

    public static void SeedTasks(BloggingContext db)
    {
        // Undgå at indsætte igen hver gang du kører programmet
        if (db.Tasks.Any())
            return;

        var produceSoftware = new TaskItem
        {
            Name = "Produce software",
            Todos =
            {
                new Todo { Name = "Write code", IsComplete = false },
                new Todo { Name = "Compile source", IsComplete = false },
                new Todo { Name = "Test program", IsComplete = false }
            }
        };

        var brewCoffee = new TaskItem
        {
            Name = "Brew coffee",
            Todos =
            {
                new Todo { Name = "Pour water", IsComplete = false },
                new Todo { Name = "Pour coffee", IsComplete = false },
                new Todo { Name = "Turn on", IsComplete = false }
            }
        };

        db.Tasks.AddRange(produceSoftware, brewCoffee);
        db.SaveChanges();
    }
};
/*


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
*/