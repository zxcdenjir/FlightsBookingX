using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;

namespace FlightsBookingX.UserControls.MenuControls.AdminControls;

public partial class AdminControl : UserControl
{
    public readonly MainView MainWindow = null!;

    public readonly User CurrentUser;

    private AircraftService _aircraftService = null!;
    private AirportService _airportService = null!;
    private FlightService _flightService = null!;
    private CityService _cityService = null!;
    private CountryService _countryService = null!;
    private TicketService _ticketService = null!;
    private UserService _userService = null!;
    private UserRoleService _userRoleService = null!;

    private List<User> _users = null!;
    private List<Ticket> _tickets = null!;
    private List<UserRole> _roles = null!;
    private List<Flight> _flights = null!;
    private List<Aircraft> _aircrafts = null!;
    private List<Airport> _airports = null!;
    private List<Country> _countries = null!;
    private List<City> _cities = null!;

    public AdminControl()
    {
        InitializeComponent();
        CurrentUser = new User { FirstName = "Иван", SecondName = "Иванов" };
        PersonalCabinetBlock.Text = $"{CurrentUser.FirstName} {CurrentUser.SecondName}";
        Loaded += async (_, _) => await LoadAsync();
    }

    public AdminControl(MainView mainWindow, User user)
    {
        InitializeComponent();
        MainWindow = mainWindow;
        CurrentUser = user;
        PersonalCabinetBlock.Text = $"{CurrentUser.FirstName} {CurrentUser.SecondName}";
        Loaded += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        List<Task> loads = [
            LoadUsersAsync(),
            LoadTicketsAsync(),
            LoadRolesAsync(),
            LoadFlightsAsync(),
            LoadAircraftsAsync(),
            LoadAirportsAsync(),
            LoadCountriesAsync(),
            LoadCitiesAsync()
        ];
        
        await Task.WhenAll(loads);
        
        if (!Design.IsDesignMode)
            await MainWindow.StopAnim();
    }

    private async Task LoadUsersAsync()
    {
        _userService = new();
        _users = (await _userService.GetAllAsync()).OrderBy(x => x.SecondName).ToList();
        UsersControl.ItemsSource = _users;
        TotalUsersBlock.Text = $"Всего пользователей: {_users.Count}";
        TotalAdminsBlock.Text = $"Администраторов: {_users.Count(x => x.RoleId == 2)}";
        TotalClientsBlock.Text = $"Клиентов: {_users.Count(x => x.RoleId == 1)}";
    }

    private async Task LoadTicketsAsync()
    {
        _ticketService = new();
        _tickets = (await _ticketService.GetAllAsync()).OrderBy(x => x.BookingDate).ToList();
        TicketsControl.ItemsSource = _tickets;
        TotalTicketsBlock.Text = $"Всего билетов: {_tickets.Count}";
    }

    private async Task LoadRolesAsync()
    {
        _userRoleService = new();
        _roles = await _userRoleService.GetAllAsync();
        RolesControl.ItemsSource = _roles;
        TotalRolesBlock.Text = $"Всего ролей: {_roles.Count}";
    }

    private async Task LoadFlightsAsync()
    {
        _flightService = new();
        _flights = (await _flightService.GetAllAsync()).OrderBy(x => x.DepartureTime).ToList();
        FlightsControl.ItemsSource = _flights;
        TotalFlightsBlock.Text = $"Всего полётов: {_flights.Count}";
    }

    private async Task LoadAircraftsAsync()
    {
        _aircraftService = new();
        _aircrafts = await _aircraftService.GetAllAsync();
        AircraftsControl.ItemsSource = _aircrafts;
        TotalAircraftsBlock.Text = $"Всего самолётов: {_aircrafts.Count}";
    }

    private async Task LoadAirportsAsync()
    {
        _airportService = new();
        _airports = await _airportService.GetAllAsync();
        AirportsControl.ItemsSource = _airports;
        TotalAirportsBlock.Text = $"Всего аэропортов: {_airports.Count}";
    }

    private async Task LoadCountriesAsync()
    {
        _countryService = new();
        _countries = await _countryService.GetAllAsync();
        CountriesControl.ItemsSource = _countries;
        TotalCountriesBlock.Text = $"Всего стран: {_countries.Count}";
    }
    
    private async Task LoadCitiesAsync()
    {
        _cityService = new();
        _cities = await _cityService.GetAllAsync();
        CitiesControl.ItemsSource = _cities;
        TotalCitiesBlock.Text = $"Всего городов: {_cities.Count}";
    }

    private async void LogoutButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.RunAnim();
        await MainWindow.ShowWelcome();
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.RunAnim();
        await MainWindow.ShowMenu(CurrentUser);
    }
    
    private async void AddUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowUserInteractions(_userService, _userRoleService, CurrentUser);
    }

    private async void EditUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var user = (sender as Button)!.DataContext as User;
        await MainWindow.ShowUserInteractions(_userService, _userRoleService, CurrentUser, user!);
    }
    
    private async void DeleteUserButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // TODO: удалятся такие билеты ... | Да или нет
        var user = ((sender as Button)!.DataContext as User)!;
        await _userService.DeleteAsync(user.Id);
        await LoadAsync();
    }
    
    private async void AddTicketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowTicketInteractions(CurrentUser, _ticketService, _flightService, _userService);
    }

    private async void EditTicketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowTicketInteractions(CurrentUser, _ticketService, _flightService, _userService, ((sender as Button)!.DataContext as Ticket)!);
    }
    
    private async void DeleteTicketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var ticket = ((sender as Button)!.DataContext as Ticket)!;
        await _ticketService.DeleteAsync(ticket.Id);
        await LoadAsync();
    }

    private async void AddRoleButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowRoleInteraction(_userRoleService, CurrentUser);
    }

    private async void EditRoleButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowRoleInteraction(_userRoleService, CurrentUser, ((sender as Button)!.DataContext as UserRole)!);
    }
    
    private async void DeleteRoleButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var role = ((sender as Button)!.DataContext as UserRole)!;
        await _userRoleService.DeleteAsync(role.Id);
        await LoadAsync();  
    }
    
    private async void AddAircraftButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowAircraftInteractions(_aircraftService, CurrentUser);
    }

    private async void EditAircraftButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var aircraft = (sender as Button)?.DataContext as Aircraft;
        if (aircraft != null)
        {
            await MainWindow.ShowAircraftInteractions(_aircraftService, CurrentUser, aircraft);
        }
    }

    private async void DeleteAircraftButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var aircraft = (sender as Button)?.DataContext as Aircraft;
        if (aircraft != null)
        {
            await _aircraftService.DeleteAsync(aircraft.Id);
        
            await LoadAsync(); 
            MainWindow.ShowSuccessNotification($"Самолет {aircraft.Model} удален");
        }
    }
    
    private async void AddAirportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowAirportInteractions(_airportService, _countryService, _cityService, CurrentUser);
    }

    private async void EditAirportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is Airport airport)
        {
            await MainWindow.ShowAirportInteractions(_airportService, _countryService, _cityService, CurrentUser, airport);
        }
    }

    private async void DeleteAirportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is Airport airport)
        {
            await _airportService.DeleteAsync(airport.Id);
            await LoadAirportsAsync();
            MainWindow.ShowSuccessNotification($"Аэропорт {airport.Code} удален");
        }
    }
}