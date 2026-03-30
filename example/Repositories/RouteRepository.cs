using TravelApp.Entities;

namespace TravelApp.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly List<Route> _routes = [];
    private int _nextId = 1;
    
    public void Add(Route route)
    {
        route.Id = _nextId++;
        _routes.Add(route);
        System.Console.WriteLine("ADD ROUTE: " + route.Name);
    }
    
    public Route? GetById(int id)
    {
        return _routes.FirstOrDefault(r => r.Id == id);
    }
    
    public IReadOnlyList<Route> GetAll()
    {
       
        return _routes;
    }
    
    public void Update(Route route)
    {
        var index = _routes.FindIndex(r => r.Id == route.Id);
        if (index != -1)
        {
            _routes[index] = route;
        }
    }
    
    public void Delete(int id)
    {
        var route = GetById(id);
        if (route != null)
        {
            _routes.Remove(route);
        }
    }
    
    public bool Exists(int id)
    {
        return _routes.Any(r => r.Id == id);
    }
}