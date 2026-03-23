using TravelApp.Entities;

namespace TravelApp.Repositories;

public interface IRouteRepository
{
    void Add(Route route);
    Route? GetById(int id);
    IReadOnlyList<Route> GetAll();
    void Update(Route route);
    void Delete(int id);
    bool Exists(int id);
}