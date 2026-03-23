using TravelApp.Repositories;
using TravelApp.Services;
using TravelApp.Console;

class Program
{
    static void Main(string[] args)
    {
        // Создание репозиториев
        ICityRepository cityRepository = new CityRepository();
        IHaltRepository haltRepository = new HaltRepository();
        IRouteRepository routeRepository = new RouteRepository();
        
        // Создание сервисов с внедрением зависимостей
        ICityService cityService = new CityService(cityRepository);
        IHaltService haltService = new HaltService(haltRepository, cityRepository, routeRepository);
        IRouteService routeService = new RouteService(routeRepository, haltRepository, cityRepository);
        
        // Создание и запуск меню
        AppMenu menu = new AppMenu(cityService, routeService, haltService);
        menu.Run();
    }
}