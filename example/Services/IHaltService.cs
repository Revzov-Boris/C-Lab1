using TravelApp.Entities;

namespace TravelApp.Services;

public interface IHaltService
{
    Halt? GetHalt(int id);
    IReadOnlyList<Halt> GetAllHalts();
    IReadOnlyList<Halt> GetHaltsByRoute(int routeId);
    IReadOnlyList<Halt> GetHaltsByCity(int cityId);
    IReadOnlyList<Halt> GetHaltsByDateRange(DateTime startDate, DateTime endDate);
    IReadOnlyList<Halt> GetHaltsByCityAndDateRange(int cityId, DateTime startDate, DateTime endDate);
    IReadOnlyList<Halt> GetHaltsSortedByTime();
    IReadOnlyDictionary<City, int> GetHaltsCountByCity();
    IReadOnlyDictionary<int, int> GetHaltsCountByRoute();
    IReadOnlyList<Halt> GetLatestHalts(int count);
    void UpdateHalt(int id, int cityId, DateTime haltTime);
    void DeleteHalt(int id);
    bool HaltExists(int id);
}