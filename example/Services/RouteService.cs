using TravelApp.Entities;
using TravelApp.Repositories;

namespace TravelApp.Services;

public class RouteService : IRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IHaltRepository _haltRepository;
    private readonly ICityRepository _cityRepository;
    
    public RouteService(IRouteRepository routeRepository, IHaltRepository haltRepository, ICityRepository cityRepository)
    {
        _routeRepository = routeRepository;
        _haltRepository = haltRepository;
        _cityRepository = cityRepository;
    }
    
    public void CreateRoute(string name, string description)
    {
        var route = new Route
        {
            Name = name,
            Description = description
        };
        _routeRepository.Add(route);
    }
    
    public Route? GetRoute(int id)
    {
        return _routeRepository.GetById(id);
    }
    
    public IReadOnlyList<Route> GetAllRoutes()
    {
        return _routeRepository.GetAll();
    }
    
    public void AddHaltToRoute(int routeId, int cityId, DateTime haltTime)
    {
        var route = _routeRepository.GetById(routeId);
        if (route == null) 
            throw new InvalidOperationException("Маршрут не найден");
        
        var city = _cityRepository.GetById(cityId);
        if (city == null) 
            throw new InvalidOperationException("Город не найден");
        
        var halt = new Halt
        {
            CityId = cityId,
            HaltTime = haltTime,
            RouteId = routeId
        };
        
        _haltRepository.Add(halt);
        route.HaltIds.Add(halt.Id);
        _routeRepository.Update(route);
    }
    
    public void RemoveHaltFromRoute(int routeId, int haltId)
    {
        var route = _routeRepository.GetById(routeId);
        if (route != null)
        {
            route.HaltIds.Remove(haltId);
            _haltRepository.Delete(haltId);
            _routeRepository.Update(route);
        }
    }
    
    //  получение остановок маршрута с сортировкой по времени
    public IReadOnlyList<Halt> GetRouteHalts(int routeId)
    {
        var route = _routeRepository.GetById(routeId);
        if (route == null) 
            return new List<Halt>().AsReadOnly();
        
        return route.HaltIds
            .Select(id => _haltRepository.GetById(id))
            .Where(halt => halt != null)
            .Cast<Halt>()
            .OrderBy(halt => halt.HaltTime)
            .ToList()
            .AsReadOnly();
    }
    
    //  маршруты, проходящие через конкретный город
    public IReadOnlyList<Route> GetRoutesByCity(int cityId)
    {
        var haltsInCity = _haltRepository.GetByCityId(cityId);
        
        // Фильтруем только те остановки, у которых есть RouteId (не null)
        var routeIds = haltsInCity
            .Where(h => h.RouteId.HasValue) // Отфильтровываем null значения
            .Select(h => h.RouteId.Value)   // Безопасно получаем значение
            .Distinct();
        
        return routeIds
            .Select(id => _routeRepository.GetById(id))
            .Where(route => route != null)
            .Cast<Route>()
            .ToList()
            .AsReadOnly();
    }


    
    //  сортировка маршрутов по количеству остановок
    public IReadOnlyList<Route> GetRoutesSortedByHaltsCount()
    {
        return _routeRepository.GetAll()
            .OrderByDescending(r => r.TotalHalts)
            .ToList()
            .AsReadOnly();
    }
    
    //  маршруты за определенный период
    public IReadOnlyList<Route> GetRoutesByDateRange(DateTime startDate, DateTime endDate)
    {
        var haltsInRange = _haltRepository.GetAll()
            .Where(h => h.HaltTime >= startDate && h.HaltTime <= endDate);
        
        // Фильтруем только те остановки, у которых есть RouteId (не null)
        var routeIds = haltsInRange
            .Where(h => h.RouteId.HasValue) // Отфильтровываем null значения
            .Select(h => h.RouteId.Value)   // Безопасно получаем значение
            .Distinct();
        
        return routeIds
            .Select(id => _routeRepository.GetById(id))
            .Where(route => route != null)
            .Cast<Route>()
            .ToList()
            .AsReadOnly();
    }
    
    //  группировка остановок по городам с подсчетом количества
    public IReadOnlyDictionary<City, int> GetHaltsCountByCity()
    {
        var halts = _haltRepository.GetAll();
        var cities = _cityRepository.GetAll();
        
        var result = halts
            .GroupBy(h => h.CityId)
            .Select(g => new
            {
                City = cities.FirstOrDefault(c => c.Id == g.Key),
                Count = g.Count()
            })
            .Where(item => item.City != null)
            .ToDictionary(
                item => item.City!,
                item => item.Count
            );
        
        return result.AsReadOnly();
    }
    
    public void DeleteRoute(int id)
    {
        var route = _routeRepository.GetById(id);
        if (route != null)
        {
            foreach (var haltId in route.HaltIds)
            {
                _haltRepository.Delete(haltId);
            }
            _routeRepository.Delete(id);
        }
    }
}