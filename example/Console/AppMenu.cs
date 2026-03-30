using TravelApp.Services;
using TravelApp.Entities;

namespace TravelApp.Console;

public class AppMenu
{
    private readonly ICityService _cityService;
    private readonly IRouteService _routeService;
    private readonly IHaltService _haltService;
    
    public AppMenu(ICityService cityService, IRouteService routeService, IHaltService haltService)
    {
        _cityService = cityService;
        _routeService = routeService;
        _haltService = haltService;
    }
    
    public void Run()
    {
        // Добавляем тестовые данные для демонстрации
        AddTestData();
        
        while (true)
        {
            //System.Console.Clear();
            System.Console.WriteLine("=== Управление маршрутами ===");
            System.Console.WriteLine("1. Города - отсортированные по часовому поясу");
            System.Console.WriteLine("2. Города - поиск по названию");
            System.Console.WriteLine("3. Маршруты - отсортированные по количеству остановок");
            System.Console.WriteLine("4. Маршруты - проходящие через конкретный город");
            System.Console.WriteLine("6. Статистика - количество остановок по городам");
            System.Console.WriteLine("0. Выход");
            System.Console.Write("Выберите опцию: ");
            
            var choice = System.Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    ShowCitiesSortedByTime();
                    break;
                case "2":
                    SearchCitiesByName();
                    break;
                case "3":
                    ShowRoutesSortedByHalts();
                    break;
                case "4":
                    ShowRoutesByCity();
                    break;
                case "6":
                    ShowHaltsCountByCity();
                    break;
                case "0":
                    System.Console.WriteLine("До свидания!");
                    return;
                default:
                    System.Console.WriteLine("Неверный выбор.");
                    break;
            }
            
            System.Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            System.Console.ReadKey();
        }
    }
    
    // Запрос 1: Города, отсортированные по часовому поясу
    private void ShowCitiesSortedByTime()
    {
        var cities = _cityService.GetCitiesSortedByTimeOffset();
        
        System.Console.WriteLine("\n=== Города, отсортированные по часовому поясу ===");
        
        if (!cities.Any())
        {
            System.Console.WriteLine("Нет городов");
            return;
        }
        
        foreach (var city in cities)
        {
            System.Console.WriteLine($"{city.Name} | {city.Country} | UTC{(city.TimeOffsetHours >= 0 ? "+" : "")}{city.TimeOffsetHours}");
        }
    }
    
    // Запрос 2: Поиск городов по названию
    private void SearchCitiesByName()
    {
        System.Console.Write("\nВведите название города для поиска: ");
        var searchTerm = System.Console.ReadLine() ?? string.Empty;
        
        var cities = _cityService.SearchCitiesByName(searchTerm);
        
        System.Console.WriteLine($"\nРезультаты поиска по '{searchTerm}':");
        
        if (!cities.Any())
        {
            System.Console.WriteLine("Ничего не найдено");
            return;
        }
        
        foreach (var city in cities)
        {
            System.Console.WriteLine($"{city.Name} | {city.Country}, {city.Region} | UTC{(city.TimeOffsetHours >= 0 ? "+" : "")}{city.TimeOffsetHours}");
        }
    }
    
    // Запрос 3: Маршруты, отсортированные по количеству остановок
    private void ShowRoutesSortedByHalts()
    {
        var routes = _routeService.GetRoutesSortedByHaltsCount();
        
        System.Console.WriteLine("\n=== Маршруты по убыванию количества остановок ===");
        
        if (!routes.Any())
        {
            System.Console.WriteLine("Нет маршрутов");
            return;
        }
        
        foreach (var route in routes)
        {
            var haltsCount = _routeService.GetRouteHalts(route.Id).Count;
            System.Console.WriteLine($"{route.Name} - {haltsCount} остановок | {route.Description}");
        }
    }
    
    // Запрос 4: Маршруты, проходящие через конкретный город
    private void ShowRoutesByCity()
    {
        var cities = _cityService.GetAllCities();
        
        if (!cities.Any())
        {
            System.Console.WriteLine("\nНет городов");
            return;
        }
        
        System.Console.WriteLine("\nДоступные города:");
        foreach (var сcity in cities)
        {
            System.Console.WriteLine($"ID: {сcity.Id} - {сcity.Name}");
        }
        
        System.Console.Write("\nВведите ID города: ");
        var cityId = int.Parse(System.Console.ReadLine() ?? "0");
        
        var city = _cityService.GetCity(cityId);
        if (city == null)
        {
            System.Console.WriteLine("Город не найден!");
            return;
        }
        
        var routes = _routeService.GetRoutesByCity(cityId);
        
        System.Console.WriteLine($"\nМаршруты, проходящие через город {city.Name}:");
        
        if (!routes.Any())
        {
            System.Console.WriteLine("Нет маршрутов через этот город");
            return;
        }
        
        foreach (var route in routes)
        {
            var halts = _routeService.GetRouteHalts(route.Id);
            var haltsInCity = halts.Where(h => h.CityId == cityId);
            
            System.Console.WriteLine($"\n{route.Name}:");
            foreach (var halt in haltsInCity)
            {
                System.Console.WriteLine($"  - Остановка в {halt.HaltTime:dd.MM.yyyy HH:mm}");
            }
        }
    }
    
    
    // Запрос 6: Статистика остановок по городам
    private void ShowHaltsCountByCity()
    {
        var stats = _haltService.GetHaltsCountByCity();
        
        System.Console.WriteLine("\n=== Статистика остановок по городам ===");
        
        if (!stats.Any())
        {
            System.Console.WriteLine("Нет данных");
            return;
        }
        
        foreach (var stat in stats)
        {
            System.Console.WriteLine($"{stat.Key.Name} ({stat.Key.Country}) - {stat.Value} остановок");
        }
    }
    
    // Добавление тестовых данных для демонстрации
    private void AddTestData()
    {
        System.Console.WriteLine("Добавление тестовых данных...");
        
        // Добавляем города
        _cityService.CreateCity("Москва", 3, "Россия", "Центральный");
        _cityService.CreateCity("Санкт-Петербург", 3, "Россия", "Северо-Западный");
        _cityService.CreateCity("Новосибирск", 7, "Россия", "Восточная Сибирь");
        _cityService.CreateCity("Владивосток", 13, "Россия", "Дальний Восток");
        _cityService.CreateCity("Лондон", 0, "Великобритания", "Англия");
        _cityService.CreateCity("Нью-Йорк", -5, "США", "Нью-Йорк");
        _cityService.CreateCity("Токио", 9, "Япония", "Канто");
        _cityService.CreateCity("Пекин", 9, "Китай", "Северо-Восток");
        _cityService.CreateCity("Дели", 5, "Индия", "Центр");
        _cityService.CreateCity("Париж", 1, "Франция", "Иль-де-Франс");
        
        // Добавляем маршруты
        _routeService.CreateRoute("Транссибирская магистраль", "Москва - Владивосток");
        _routeService.CreateRoute("Европейский туризм", "Лондон - Париж - Берлин");
        _routeService.CreateRoute("Восточный путь", "Пекин - Дели");
        
        // Получаем ID городов для добавления остановок
        var moscow = _cityService.GetAllCities().First(c => c.Name == "Москва");
        var spb = _cityService.GetAllCities().First(c => c.Name == "Санкт-Петербург");
        var novosibirsk = _cityService.GetAllCities().First(c => c.Name == "Новосибирск");
        var london = _cityService.GetAllCities().First(c => c.Name == "Лондон");
        var paris = _cityService.GetAllCities().First(c => c.Name == "Париж");
        var tokyo = _cityService.GetAllCities().First(c => c.Name == "Токио");
        var deli = _cityService.GetAllCities().First(c => c.Name == "Дели");
        var pekin = _cityService.GetAllCities().First(c => c.Name == "Пекин");
        


        System.Console.WriteLine("FROM_APP");
        foreach (var route in _routeService.GetAllRoutes())
        {
            System.Console.WriteLine("ПУТЬ");
            System.Console.WriteLine($"{route.Name} - {route.TotalHalts} остановок | {route.Description}");
        }
        var transsib = _routeService.GetAllRoutes().First(r => r.Name == "Транссибирская магистраль");
        var europeTour = _routeService.GetAllRoutes().First(r => r.Name == "Европейский туризм");
        var eastTour = _routeService.GetAllRoutes().First(r => r.Name == "Восточный путь");
        
        _routeService.AddHaltToRoute(transsib.Id, moscow.Id, DateTime.Now.AddDays(-5).AddHours(10));
        _routeService.AddHaltToRoute(transsib.Id, spb.Id, DateTime.Now.AddDays(-4).AddHours(15));
        _routeService.AddHaltToRoute(transsib.Id, novosibirsk.Id, DateTime.Now.AddDays(-2).AddHours(8));
        
        _routeService.AddHaltToRoute(europeTour.Id, london.Id, DateTime.Now.AddDays(-3).AddHours(9));
        _routeService.AddHaltToRoute(europeTour.Id, paris.Id, DateTime.Now.AddDays(-2).AddHours(14));
        _routeService.AddHaltToRoute(europeTour.Id, paris.Id, DateTime.Now.AddDays(-1).AddHours(10));
        
  
        _routeService.AddHaltToRoute(eastTour.Id, tokyo.Id, DateTime.Now.AddDays(-4).AddHours(11));
        _routeService.AddHaltToRoute(eastTour.Id, pekin.Id, DateTime.Now.AddDays(-2).AddHours(9));
        _routeService.AddHaltToRoute(eastTour.Id, deli.Id, DateTime.Now.AddDays(-1).AddHours(1));

        
        // Добавляем несколько остановок без маршрута
        var halt1 = new Halt { CityId = moscow.Id, HaltTime = DateTime.Now.AddDays(-1).AddHours(20), RouteId = null };
        var halt2 = new Halt { CityId = london.Id, HaltTime = DateTime.Now.AddDays(-1).AddHours(15), RouteId = null };
        var halt3 = new Halt { CityId = tokyo.Id, HaltTime = DateTime.Now.AddDays(-1).AddHours(5), RouteId = null };
        
        _haltService.AddHalt(halt1);
        _haltService.AddHalt(halt2);
        _haltService.AddHalt(halt3);
        
        System.Console.WriteLine("Тестовые данные добавлены!");
    }
}