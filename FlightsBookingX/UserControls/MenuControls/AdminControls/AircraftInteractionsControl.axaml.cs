using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;

namespace FlightsBookingX.UserControls.MenuControls.AdminControls;

public partial class AircraftInteractionsControl : UserControl
{
    private MainView _mainWindow = null!;
    private AircraftService _aircraftService = null!;
    private User _currentUser = null!;
    private Aircraft _aircraft = null!;

    public AircraftInteractionsControl()
    {
        InitializeComponent();
        _aircraft = new Aircraft { Manufacturer = "", Model = "", SeatsCount = 100 };
        DataContext = _aircraft;
    }

    public AircraftInteractionsControl(MainView mainWindow, AircraftService aircraftService, User currentUser)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _aircraftService = aircraftService;
        _currentUser = currentUser;
        _aircraft = new Aircraft { SeatsCount = 100 };
        DataContext = _aircraft;
        SaveAircraftButton.IsVisible = false;
    }

    public AircraftInteractionsControl(MainView mainWindow, AircraftService aircraftService, User currentUser, Aircraft aircraft)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _aircraftService = aircraftService;
        _currentUser = currentUser;
        _aircraft = aircraft;
        DataContext = _aircraft;
        AddAircraftButton.IsVisible = false;
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.ShowAdmin(_currentUser);
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!Validate()) return;

        try
        {
            await _mainWindow.RunAnim();
            await _aircraftService.UpdateAsync(_aircraft);
            _mainWindow.ShowSuccessNotification($"Данные о самолете {_aircraft.Model} обновлены!");
            await _mainWindow.ShowAdmin(_currentUser);
        }
        catch
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowErrorNotification("Ошибка при обновлении данных.");
        }
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!Validate()) return;

        try
        {
            await _mainWindow.RunAnim();
            await _aircraftService.CreateAsync(_aircraft);
            _mainWindow.ShowSuccessNotification($"Самолет {_aircraft.Model} успешно добавлен!");
            await _mainWindow.ShowAdmin(_currentUser);
        }
        catch
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowErrorNotification("Ошибка при создании записи.");
        }
    }

    private bool Validate()
    {
        ValidationContext vContext = new ValidationContext(_aircraft);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(_aircraft, vContext, results, true))
        {
            _mainWindow.ShowWarningNotification("Заполните все поля корректно!");
            return false;
        }
        return true;
    }
}