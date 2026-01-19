namespace EFGetStartedAgain;

public class Team
{
    public int TeamId { get; set; }
    public string TeamName { get; set; } = "";

    public List<TeamWorker> TeamWorkers { get; set; } = new();
}