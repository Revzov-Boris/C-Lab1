namespace TravelApp.Entities;

public class Halt
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public DateTime HaltTime { get; set; }
    public int? RouteId { get; set; } // nullable для связи с маршрутом
    
    public override string ToString()
    {
        return $"Остановка в городе ID:{CityId} в {HaltTime:dd.MM.yyyy HH:mm}";
    }
}