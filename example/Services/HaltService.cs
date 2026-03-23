using TravelApp.Entities;
using TravelApp.Repositories;

namespace TravelApp.Services;

public class HaltService : IHaltService
{
    private readonly IHaltRepository _haltRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IRouteRepository _routeRepository;
    
    public HaltService(IHaltRepository haltRepository, ICityRepository cityRepository, IRouteRepository routeRepository)
    {
        _haltRepository = haltRepository;
        _cityRepository = cityRepository;
        _routeRepository = routeRepository;
    }
    
    public Halt? GetHalt(int id)
    {
        return _haltRepository.GetById(id);
    }
    
    public IReadOnlyList<Halt> GetAllHalts()
    {
        return _haltRepository.GetAll();
    }
    
    //  остановки по маршруту с сортировкой по времени
    public IReadOnlyList<Halt> GetHaltsByRoute(int routeId)
    {
        return _haltRepository.GetByRouteId(routeId)
            .OrderBy(h => h.HaltTime)
            .ToList()
            .AsReadOnly();
    }
    
    //  остановки по городу с сортировкой по времени
    public IReadOnlyList<Halt> GetHaltsByCity(int cityId)
    {
        return _haltRepository.GetByCityId(cityId)
            .OrderBy(h => h.HaltTime)
            .ToList()
            .AsReadOnly();
    }
    
    //  остановки за период
    public IReadOnlyList<Halt> GetHaltsByDateRange(DateTime startDate, DateTime endDate)
    {
        return _haltRepository.GetAll()
            .Where(h => h.HaltTime >= startDate && h.HaltTime <= endDate)
            .OrderBy(h => h.HaltTime)
            .ToList()
            .AsReadOnly();
    }
    
    //  остановки по городу за период
    public IReadOnlyList<Halt> GetHaltsByCityAndDateRange(int cityId, DateTime startDate, DateTime endDate)
    {
        return _haltRepository.GetAll()
            .Where(h => h.CityId == cityId && h.HaltTime >= startDate && h.HaltTime <= endDate)
            .OrderBy(h => h.HaltTime)
            .ToList()
            .AsReadOnly();
    }
    
    //  все остановки, отсортированные по времени
    public IReadOnlyList<Halt> GetHaltsSortedByTime()
    {
        return _haltRepository.GetAll()
            .OrderBy(h => h.HaltTime)
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
            .OrderByDescending(item => item.Count)
            .ToDictionary(
                item => item.City!,
                item => item.Count
            );
        
        return result.AsReadOnly();
    }
    
    //  группировка остановок по маршрутам с подсчетом количества
    public IReadOnlyDictionary<int, int> GetHaltsCountByRoute()
    {
        return _haltRepository.GetAll()
            .Where(h => h.RouteId.HasValue)
            .GroupBy(h => h.RouteId!.Value)
            .OrderByDescending(g => g.Count())
            .ToDictionary(
                g => g.Key,
                g => g.Count()
            )
            .AsReadOnly();
    }
    
    //  последние N остановок
    public IReadOnlyList<Halt> GetLatestHalts(int count)
    {
        return _haltRepository.GetAll()
            .OrderByDescending(h => h.HaltTime)
            .Take(count)
            .ToList()
            .AsReadOnly();
    }
    
    public void UpdateHalt(int id, int cityId, DateTime haltTime)
    {
        var halt = _haltRepository.GetById(id);
        if (halt == null)
            throw new InvalidOperationException($"Остановка с ID {id} не найдена");
        
        var city = _cityRepository.GetById(cityId);
        if (city == null)
            throw new InvalidOperationException($"Город с ID {cityId} не найден");
        
        halt.CityId = cityId;
        halt.HaltTime = haltTime;
        _haltRepository.Update(halt);
    }
    
    public void DeleteHalt(int id)
    {
        var halt = _haltRepository.GetById(id);
        if (halt == null)
            throw new InvalidOperationException($"Остановка с ID {id} не найдена");
        
        // Если остановка принадлежит маршруту, удаляем её из маршрута
        if (halt.RouteId.HasValue)
        {
            var route = _routeRepository.GetById(halt.RouteId.Value);
            if (route != null)
            {
                route.HaltIds.Remove(id);
                _routeRepository.Update(route);
            }
        }
        
        _haltRepository.Delete(id);
    }
    
    public bool HaltExists(int id)
    {
        return _haltRepository.GetById(id) != null;
    }


     public void AddHalt(Halt halt)
    {
        // Валидация: проверяем существование города
        var city = _cityRepository.GetById(halt.CityId);
        if (city == null)
            throw new InvalidOperationException($"Город с ID {halt.CityId} не найден");
        
        // Если остановка привязана к маршруту, проверяем существование маршрута
        if (halt.RouteId.HasValue)
        {
            var route = _routeRepository.GetById(halt.RouteId.Value);
            if (route == null)
                throw new InvalidOperationException($"Маршрут с ID {halt.RouteId} не найден");
        }
        
        // Добавляем остановку в репозиторий
        _haltRepository.Add(halt);
        
        // Если остановка привязана к маршруту, добавляем её ID в маршрут
        if (halt.RouteId.HasValue)
        {
            var route = _routeRepository.GetById(halt.RouteId.Value);
            if (route != null && !route.HaltIds.Contains(halt.Id))
            {
                route.HaltIds.Add(halt.Id);
                _routeRepository.Update(route);
            }
        }
    }
}