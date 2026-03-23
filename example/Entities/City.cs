namespace TravelApp.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int TimeOffsetHours { get; set; } // на сколько часов опережает мировое время
    public string Country { get; set; } = "";
    public string Region { get; set; } = "";
    
    public string FullAddress => $"{Country}, {Region}, {Name}";
    
    public override string ToString()
    {
        return $"{Name} ({Country}, {Region}) | UTC+{TimeOffsetHours}";
    }
}