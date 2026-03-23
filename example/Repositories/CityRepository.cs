using TravelApp.Entities;

namespace TravelApp.Repositories;

public class CityRepository : ICityRepository
{
    private readonly List<City> _cities = [];
    private int _nextId = 1;
    
    public void Add(City city)
    {
        city.Id = _nextId++;
        _cities.Add(city);
    }
    
    public City? GetById(int id)
    {
        return _cities.FirstOrDefault(c => c.Id == id);
    }
    
    public IReadOnlyList<City> GetAll()
    {
        return _cities.AsReadOnly();
    }
    
    public void Update(City city)
    {
        var index = _cities.FindIndex(c => c.Id == city.Id);
        if (index != -1)
        {
            _cities[index] = city;
        }
    }
    
    public void Delete(int id)
    {
        var city = GetById(id);
        if (city != null)
        {
            _cities.Remove(city);
        }
    }
    
    public bool Exists(int id)
    {
        return _cities.Any(c => c.Id == id);
    }
}