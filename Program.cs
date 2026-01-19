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
        SeedWorkers(db);

        PrintIncompleteTasksAndTodos();

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
        if (db.Tasks.Any())
        return;

        var produceSoftware = new TaskItem
        {
            Name = "Produce software",
            Todos =
            {
                new Todo { Name = "Write code", IsComplete = false },
                new Todo { Name = "Compile source", IsComplete = true },
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
                new Todo { Name = "Turn on", IsComplete = true }
            }
        };

        db.Tasks.AddRange(produceSoftware, brewCoffee);
        db.SaveChanges();
    }

    public static void SeedWorkers(BloggingContext db)
{

    // undgå at seede flere gange
    if (db.TeamWorkers.Any()) return;

    // Teams
    var frontend = new Team { TeamName = "Frontend" };
    var backend = new Team { TeamName = "Backend" };
    var testere = new Team { TeamName = "Testere" };

    // Workers
    var steen = new Worker { WorkerName = "Steen Secher" };
    var ejvind = new Worker { WorkerName = "Ejvind Møller" };
    var konrad = new Worker { WorkerName = "Konrad Sommer" };
    var sofus = new Worker { WorkerName = "Sofus Lotus" };
    var remo = new Worker { WorkerName = "Remo Lademann" };
    var ella = new Worker { WorkerName = "Ella Fanth" };
    var anne = new Worker { WorkerName = "Anne Dam" };

    // Join rows (TeamWorkers)
    db.TeamWorkers.AddRange(
        new TeamWorker { Team = frontend, Worker = steen },
        new TeamWorker { Team = frontend, Worker = ejvind },
        new TeamWorker { Team = frontend, Worker = konrad },

        new TeamWorker { Team = backend, Worker = konrad },
        new TeamWorker { Team = backend, Worker = sofus },
        new TeamWorker { Team = backend, Worker = remo },

        new TeamWorker { Team = testere, Worker = ella },
        new TeamWorker { Team = testere, Worker = anne },
        new TeamWorker { Team = testere, Worker = steen }
    );

    db.SaveChanges();
}

    public static void PrintIncompleteTasksAndTodos()
{
    using (var context = new BloggingContext())
    {
        var tasks = context.Tasks
            .Include(t => t.Todos)
            .Where(t => t.Todos.Any(td => !td.IsComplete))   // task har mindst én ufærdig todo
            .ToList();

        foreach (var task in tasks)
        {
            Console.WriteLine($"Task: {task.Name}");

            foreach (var todo in task.Todos.Where(td => !td.IsComplete)) // kun ufærdige todos
            {
                Console.WriteLine($"  - [ ] {todo.Name}");
            }

            Console.WriteLine();
        }
    }
}
};
