using TravelApp.Entities;
using TravelApp.Repositories;

namespace TravelApp.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    
    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    
    public void CreateCity(string name, int timeOffset, string country, string region)
    {
        var city = new City
        {
            Name = name,
            TimeOffsetHours = timeOffset,
            Country = country,
            Region = region
        };
        _cityRepository.Add(city);
    }
    
    public City? GetCity(int id)
    {
        return _cityRepository.GetById(id);
    }
    
    public IReadOnlyList<City> GetAllCities()
    {
        return _cityRepository.GetAll();
    }
    
    //  поиск по имени
    public IReadOnlyList<City> SearchCitiesByName(string searchTerm)
    {
        return _cityRepository.GetAll()
            .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }
    
    //  фильтрация по стране
    public IReadOnlyList<City> GetCitiesByCountry(string country)
    {
        return _cityRepository.GetAll()
            .Where(c => c.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }
    
    //  сортировка по часовому поясу
    public IReadOnlyList<City> GetCitiesSortedByTimeOffset()
    {
        return _cityRepository.GetAll()
            .OrderBy(c => c.TimeOffsetHours)
            .ToList()
            .AsReadOnly();
    }
    
    public void UpdateCity(int id, string name, int timeOffset, string country, string region)
    {
        var city = _cityRepository.GetById(id);
        if (city != null)
        {
            city.Name = name;
            city.TimeOffsetHours = timeOffset;
            city.Country = country;
            city.Region = region;
            _cityRepository.Update(city);
        }
    }
    
    public void DeleteCity(int id)
    {
        _cityRepository.Delete(id);
    }
}