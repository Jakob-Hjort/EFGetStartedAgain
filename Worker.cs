namespace EFGetStartedAgain;

public class Worker
{
    public int WorkerId { get; set; }
    public string WorkerName { get; set; } = "";

    public List<TeamWorker> TeamWorkers { get; set; } = new();

    public List<Todo> Todos {get; set;} = new();

    public int? CurrentTodoId {get; set;}
    public Todo? CurrentTodo {get; set;}
}
