namespace EFGetStartedAgain;

public class Team
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = "";

    public List<TeamWorker> TeamWorkers { get; set; } = new();

    public List<TaskItem> Tasks {get; set;} = new();

    public int? CurrentTaskId {get; set;}
    public TaskItem? CurrentTask {get; set;}
}