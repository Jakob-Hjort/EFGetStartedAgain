using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFGetStartedAgain;

public class Program
{
    public static void Main()
    {
        using var db = new BloggingContext();

        // Sørger for at databasen oprettes/er opdateret
        db.Database.Migrate();

        // Seed data
        //SeedTasks(db);
        SeedWorkers(db);
        SeedOpgave24(db);

        PrintIncompleteTasksAndTodos(db);

            Console.WriteLine("Counts:");
            Console.WriteLine($"Teams: {db.Teams.Count()}");
            Console.WriteLine($"Workers: {db.Workers.Count()}");
            Console.WriteLine($"TeamWorkers: {db.TeamWorkers.Count()}");
            Console.WriteLine($"Tasks: {db.Tasks.Count()}");
            Console.WriteLine($"Todos: {db.Todos.Count()}");

        Console.WriteLine("DB path: " + db.DbPath);

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

public static void SeedOpgave24(BloggingContext db)
{
    // Undgå at seede igen
    if (db.Tasks.Any(t => t.Name == "Frontend - Sprint")) return;

    // Hent teams og workers fra DB (som SeedWorkers lige har oprettet)
    var frontend = db.Teams.Single(t => t.TeamName == "Frontend");
    var backend  = db.Teams.Single(t => t.TeamName == "Backend");
    var testere  = db.Teams.Single(t => t.TeamName == "Testere");

    var steen  = db.Workers.Single(w => w.WorkerName == "Steen Secher");
    var ejvind = db.Workers.Single(w => w.WorkerName == "Ejvind Møller");
    var konrad = db.Workers.Single(w => w.WorkerName == "Konrad Sommer");
    var sofus  = db.Workers.Single(w => w.WorkerName == "Sofus Lotus");
    var remo   = db.Workers.Single(w => w.WorkerName == "Remo Lademann");
    var ella   = db.Workers.Single(w => w.WorkerName == "Ella Fanth");
    var anne   = db.Workers.Single(w => w.WorkerName == "Anne Dam");

    // 1 task pr team (så "alle teams har en opgave")
    var t1 = new TaskItem { Name = "Frontend - Sprint", TeamId = frontend.TeamId };
    var t2 = new TaskItem { Name = "Backend - Sprint",  TeamId = backend.TeamId };
    var t3 = new TaskItem { Name = "Testing - Sprint",  TeamId = testere.TeamId };

    db.Tasks.AddRange(t1, t2, t3);
    db.SaveChanges(); // giver TaskItemId

    // Sæt CurrentTask på teams
    frontend.CurrentTaskId = t1.TaskItemId;
    backend.CurrentTaskId  = t2.TaskItemId;
    testere.CurrentTaskId  = t3.TaskItemId;

    // Todos pr task, og hver todo tildeles en worker
    var todos = new[]
    {
        new Todo { Name = "Write code",     IsComplete = false, TaskItemId = t1.TaskItemId, WorkerId = steen.WorkerId },
        new Todo { Name = "Compile source", IsComplete = true,  TaskItemId = t1.TaskItemId, WorkerId = ejvind.WorkerId },
        new Todo { Name = "Test program",   IsComplete = false, TaskItemId = t1.TaskItemId, WorkerId = konrad.WorkerId },

        new Todo { Name = "Pour water",     IsComplete = false, TaskItemId = t2.TaskItemId, WorkerId = sofus.WorkerId },
        new Todo { Name = "Pour coffee",    IsComplete = false, TaskItemId = t2.TaskItemId, WorkerId = remo.WorkerId },
        new Todo { Name = "Turn on",        IsComplete = true,  TaskItemId = t2.TaskItemId, WorkerId = konrad.WorkerId },

        new Todo { Name = "Verify UI",      IsComplete = false, TaskItemId = t3.TaskItemId, WorkerId = ella.WorkerId },
        new Todo { Name = "Regression test",IsComplete = false, TaskItemId = t3.TaskItemId, WorkerId = anne.WorkerId },
        new Todo { Name = "Report bugs",    IsComplete = false, TaskItemId = t3.TaskItemId, WorkerId = steen.WorkerId },
    };

    db.Todos.AddRange(todos);
    db.SaveChanges();

    // Sæt CurrentTodo på workers (bare et eksempel)
    steen.CurrentTodoId  = todos.First(t => t.WorkerId == steen.WorkerId).TodoId;
    konrad.CurrentTodoId = todos.First(t => t.WorkerId == konrad.WorkerId).TodoId;

    db.SaveChanges();
    }

    public static void PrintIncompleteTasksAndTodos(BloggingContext db)
    {
    
        var tasks = db.Tasks
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
};
