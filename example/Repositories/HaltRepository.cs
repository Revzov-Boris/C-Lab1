using TravelApp.Entities;

namespace TravelApp.Repositories;

public class HaltRepository : IHaltRepository
{
    private readonly List<Halt> _halts = [];
    private int _nextId = 1;
    
    public void Add(Halt halt)
    {
        halt.Id = _nextId++;
        _halts.Add(halt);
    }
    
    public Halt? GetById(int id)
    {
        return _halts.FirstOrDefault(h => h.Id == id);
    }
    
    public IReadOnlyList<Halt> GetAll()
    {
        return _halts.AsReadOnly();
    }
    
    public IReadOnlyList<Halt> GetByRouteId(int routeId)
    {
        return _halts.Where(h => h.RouteId == routeId).ToList().AsReadOnly();
    }
    
    public IReadOnlyList<Halt> GetByCityId(int cityId)
    {
        return _halts.Where(h => h.CityId == cityId).ToList().AsReadOnly();
    }
    
    public void Update(Halt halt)
    {
        var index = _halts.FindIndex(h => h.Id == halt.Id);
        if (index != -1)
        {
            _halts[index] = halt;
        }
    }
    
    public void Delete(int id)
    {
        var halt = GetById(id);
        if (halt != null)
        {
            _halts.Remove(halt);
        }
    }
}