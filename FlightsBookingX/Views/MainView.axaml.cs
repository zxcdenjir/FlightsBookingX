using System;
using System.Threading.Tasks;
using Android.App;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using FlightsBookingX.UserControls;
using FlightsBookingX.UserControls.MenuControls;
using FlightsBookingX.UserControls.MenuControls.AdminControls;

namespace FlightsBookingX.Views;

public partial class MainView : UserControl
{
    public readonly WindowNotificationManager NotificationManager;
    
    // TODO: сделать хранение текущего пользователя здесь
    // TODO: сделать переходы через transition.Start(...)
    public MainView()
    {
        InitializeComponent();
        DataContext = this;
        NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this))
        {
            Position = NotificationPosition.BottomRight,
            MaxItems = 3
        };
        Loaded += async (_, _) => await ShowWelcome();
    }
    public void ShowErrorNotification(string message)
    {
        NotificationManager.Show(message, NotificationType.Error, TimeSpan.FromSeconds(5));
    }

    public void ShowWarningNotification(string message)
    {
        NotificationManager.Show(message, NotificationType.Warning, TimeSpan.FromSeconds(4));
    }

    public void ShowSuccessNotification(string message)
    {
        NotificationManager.Show(message, NotificationType.Success, TimeSpan.FromSeconds(3));
    }
    
    public async Task RunAnim()
    {
        IsEnabled = false;
        await Task.Yield();
        await Task.Delay(250);
    }

    public async Task StopAnim()
    {
        IsEnabled = true;
        await Task.Yield();
    }

    public async Task ShowWelcome()
    {
        MainPresenter.Content = new WelcomeControl(this);
    }

    public async Task ShowMenu(User user)
    {
        MainPresenter.Content = new MenuControl(this, user);
    }

    public async Task ShowAdmin(User user)
    {
        MainPresenter.Content = new AdminControl(this, user);
    }

    public async Task ShowSeatSelection(Flight flight, User user)
    {
        MainPresenter.Content = new SeatSelectionControl(this, flight, user);
    }
    
    public async Task ShowUserTickets(User user)
    {
        MainPresenter.Content = new ShowUserTicketsControl(this, user);
    }

    public async Task ShowUserInteractions(UserService userService, UserRoleService roleService, User currentUser,
        User user)
    {
        MainPresenter.Content = new UserInteractionsControl(this, userService, roleService, currentUser, user);
    }

    public async Task ShowUserInteractions(UserService userService, UserRoleService roleService, User currentUser)
    {
        MainPresenter.Content = new UserInteractionsControl(this, userService, roleService, currentUser);
    }

    public async Task ShowTicketInteractions(User currentUser, TicketService ticketService, FlightService flightService,
        UserService userService)
    {
        MainPresenter.Content =
            new TicketInteractionsControl(this, currentUser, ticketService, flightService, userService);
    }

    public async Task ShowTicketInteractions(User currentUser, TicketService ticketService, FlightService flightService,
        UserService userService, Ticket ticket)
    {
        MainPresenter.Content =
            new TicketInteractionsControl(this, currentUser, ticketService, flightService, userService, ticket);
    }

    public async Task ShowRoleInteraction(UserRoleService roleService, User currentUser)
    {
        MainPresenter.Content = new UserRoleInteractionsControl(this, roleService, currentUser);
    }

    public async Task ShowRoleInteraction(UserRoleService roleService, User currentUser, UserRole role)
    {
        MainPresenter.Content = new UserRoleInteractionsControl(this, roleService, currentUser, role);
    }

    public async Task ShowAircraftInteractions(AircraftService aircraftService, User currentUser)
    {
        MainPresenter.Content = new AircraftInteractionsControl(this, aircraftService, currentUser);
    }

    public async Task ShowAircraftInteractions(AircraftService aircraftService, User currentUser, Aircraft aircraft)
    {
        MainPresenter.Content = new AircraftInteractionsControl(this, aircraftService, currentUser, aircraft);
    }

    public async Task ShowAirportInteractions(AirportService service, CountryService countryService,
        CityService cityService, User currentUser)
    {
        MainPresenter.Content = new AirportInteractionsControl(this, currentUser, service, countryService, cityService);
    }

    public async Task ShowAirportInteractions(AirportService service, CountryService countryService,
        CityService cityService, User currentUser, Airport airport)
    {
        MainPresenter.Content =
            new AirportInteractionsControl(this, currentUser, service, countryService, cityService, airport);
    }
}