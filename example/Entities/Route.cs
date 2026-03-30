namespace TravelApp.Entities;

public class Route
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<int> HaltIds { get; set; } = new List<int>(); // композиция - список ID остановок
    
    public int TotalHalts => HaltIds.Count;
    
    public override string ToString()
    {
        return $"{Name} - {Description} (остановок: {TotalHalts}) " + String.Join(" -> ", HaltIds);
    }
}