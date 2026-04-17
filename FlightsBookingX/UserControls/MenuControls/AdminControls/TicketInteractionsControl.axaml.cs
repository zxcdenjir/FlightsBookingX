using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;

namespace FlightsBookingX.UserControls.MenuControls.AdminControls;

public partial class TicketInteractionsControl : UserControl
{
    private readonly MainView _mainWindow = null!;
    private readonly TicketService _ticketService = null!;
    private readonly FlightService _flightService = null!;
    private readonly UserService _userService = null!;
    private readonly User _currentUser = null!;
    private Ticket _ticket = null!;

    public TicketInteractionsControl()
    {
        InitializeComponent();
    }

    public TicketInteractionsControl(MainView mainWindow, User currentUser,
        TicketService ticketService, FlightService flightService, UserService userService)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _currentUser = currentUser;
        _ticketService = ticketService;
        _flightService = flightService;
        _userService = userService;

        _ticket = new Ticket { BookingDate = DateTime.Now, SeatNumber = 1 };
        DataContext = _ticket;
        AddButton.IsVisible = true;

        Loaded += async (_, _) => await LoadDataAsync();
    }


    public TicketInteractionsControl(MainView mainWindow, User currentUser,
        TicketService ticketService, FlightService flightService, UserService userService, Ticket ticket)
        : this(mainWindow, currentUser, ticketService, flightService, userService)
    {
        _ticket = ticket;
        DataContext = _ticket;
        AddButton.IsVisible = false;
        SaveButton.IsVisible = true;
    }

    private async Task LoadDataAsync()
    {
        FlightsComboBox.ItemsSource = await _flightService.GetAllAsync();
        UsersComboBox.ItemsSource = await _userService.GetAllAsync();
        await _mainWindow.StopAnim();
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!Validate()) return;

        try
        {
            await _ticketService.UpdateAsync(_ticket);
            _mainWindow.ShowSuccessNotification("Билет обновлен");
        }
        catch
        {
            _mainWindow.ShowErrorNotification("Ошибка сохранения");
        }

        BackButton_OnClick(null, e);
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!Validate()) return;

        try
        {
            PrepareTicketForSave();
            await _ticketService.CreateAsync(_ticket);
            _mainWindow.ShowSuccessNotification("Билет создан");
            BackButton_OnClick(null, e);
        }
        catch
        {
            _mainWindow.ShowErrorNotification("Ошибка при создании");
        }
    }

    private void PrepareTicketForSave()
    {
        if (_ticket.Flight != null)
        {
            _ticket.FlightId = _ticket.Flight.Id;
            _ticket.Flight = null!;
        }

        if (_ticket.User != null)
        {
            _ticket.UserId = _ticket.User.Id;
            _ticket.User = null!;
        }
    }

    private bool Validate()
    {
        if (_ticket.Flight == null || _ticket.User == null)
        {
            _mainWindow.ShowWarningNotification("Выберите рейс и пассажира!");
            return false;
        }

        return true;
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.ShowAdmin(_currentUser);
    }
}