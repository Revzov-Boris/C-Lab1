using TravelApp.Entities;

namespace TravelApp.Services;

public interface IRouteService
{
    void CreateRoute(string name, string description);
    Route? GetRoute(int id);
    IReadOnlyList<Route> GetAllRoutes();
    void AddHaltToRoute(int routeId, int cityId, DateTime haltTime);
    void RemoveHaltFromRoute(int routeId, int haltId);
    IReadOnlyList<Halt> GetRouteHalts(int routeId);
    IReadOnlyList<Route> GetRoutesByCity(int cityId);
    IReadOnlyList<Route> GetRoutesSortedByHaltsCount();
    IReadOnlyList<Route> GetRoutesByDateRange(DateTime startDate, DateTime endDate);
    IReadOnlyDictionary<City, int> GetHaltsCountByCity();
    void DeleteRoute(int id);
}