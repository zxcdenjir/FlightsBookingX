using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using FlightsBookingX.Views;

namespace FlightsBookingX.UserControls.MenuControls;

public partial class MenuControl : UserControl
{
    public readonly MainView MainWindow = null!;

    public readonly User CurrentUser;

    public FlightService Fs = null!;
    public AirportService As = null!;
    
    // сделать AllFlights и SortedFlights
    public List<Flight> Flights { get; set; } = null!;

    public MenuControl()
    {
        InitializeComponent();
        CurrentUser = new User { FirstName = "Иван", SecondName = "Иванов", RoleId = 1};
        PersonalCabinetBlock.Text = $"{CurrentUser.FirstName} {CurrentUser.SecondName}";
        Loaded += async (_, _) => await Load();
    }

    public MenuControl(MainView mainWindow, User user)
    {
        InitializeComponent();
        CurrentUser = user;
        MainWindow = mainWindow;
        PersonalCabinetBlock.Text = $"{CurrentUser.FirstName} {CurrentUser.SecondName}";
        AdminPanelButton.IsVisible = CurrentUser.RoleId == 2;
        Loaded += async (_, _) => await Load();
    }

    private async Task Load()
    {
        Fs = new FlightService();
        As = new AirportService();
        Flights = (await Fs.GetAllAsync());
        Sort();
        FlightsBox.ItemsSource = Flights;
        FromBox.ItemsSource = await As.GetAllAsync();
        ToBox.ItemsSource = await As.GetAllAsync();
        
        if (!Design.IsDesignMode)
            await MainWindow.StopAnim();
    }

    private async void LogoutButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.ShowWelcome();
    }

    private void Sort()
    {
        List<Flight> flights = SortingBox.SelectedIndex switch
        {
            0 => Flights.OrderBy(x => x.DepartureTime).ToList(),
            1 => Flights.OrderBy(x => x.BasePrice).ThenBy(x => x.DepartureTime).ToList(),
            _ => Flights
        };

        Flights = flights;
        
        FlightsBox.ItemsSource = Flights;
    }

    private void SortingBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded)
            return;
        Sort();
    }

    private void FromBox_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (sender is AutoCompleteBox box)
            box.IsDropDownOpen = true;
    }

    private void FromBox_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is AutoCompleteBox box)
            box.IsDropDownOpen = false;
    }

    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var fromAirport = FromBox.SelectedItem as Airport;
        var toAirport = ToBox.SelectedItem as Airport;
        if (fromAirport == null || toAirport == null)
        {
            MainWindow.NotificationManager.Show(new Notification("Внимание", "Необходимо указать место отправления и прибытия", NotificationType.Warning, TimeSpan.FromSeconds(5)));
            return;
        }
        if (fromAirport.Id == toAirport.Id)
        {
            MainWindow.NotificationManager.Show(new Notification("Ошибка", "Аэропорты отправления и прибытия не могут совпадать", NotificationType.Error, TimeSpan.FromSeconds(5)));
            return;
        }

        Sort();
        
        Flights = Flights.Where(x => x.FromAirportId == fromAirport.Id && x.ToAirportId == toAirport.Id).ToList();
        FlightsBox.ItemsSource = Flights;

        if (Flights.Count == 0)
        {
            ResetFiltersPanel.IsVisible = true;
        }
    }

    private async void ResetFiltersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ResetFiltersPanel.IsVisible = false;
        
        FromBox.Text = string.Empty;
        ToBox.Text = string.Empty;
        
        Flights = (await Fs.GetAllAsync());
        Sort();
        FlightsBox.ItemsSource = Flights;
    }

    private async void AdminPanelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await MainWindow.RunAnim();
        await MainWindow.ShowAdmin(CurrentUser);
    }

    private async void BuyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var flight = (sender as Button)!.DataContext as Flight;
        await MainWindow.ShowSeatSelection(flight!, CurrentUser);
    }

    private async void ShowTickets_OnClick(object? sender, RoutedEventArgs e)
    {
        UserPopupButton.IsChecked = false;
        await MainWindow.RunAnim();
        await MainWindow.ShowUserTickets(CurrentUser);
    }
}