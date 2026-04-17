using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;

namespace FlightsBookingX.UserControls.MenuControls.AdminControls;

public partial class AirportInteractionsControl : UserControl
{
    private readonly MainView _mainWindow = null!;
    private readonly AirportService _airportService = null!;
    private readonly CountryService _countryService = null!;
    private readonly CityService _cityService = null!;
    private readonly User _currentUser = null!;
    private Airport _airport;

    public AirportInteractionsControl()
    {
        InitializeComponent();
        _airport = new Airport();
        DataContext = _airport;
    }

    public AirportInteractionsControl(MainView mainWindow, User currentUser,
        AirportService airportService, CountryService countryService, CityService cityService)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _currentUser = currentUser;
        _airportService = airportService;
        _countryService = countryService;
        _cityService = cityService;

        _airport = new Airport();
        DataContext = _airport;
        AddButton.IsVisible = true;

        Loaded += async (_, _) => await LoadDataAsync();
    }

    public AirportInteractionsControl(MainView mainWindow, User currentUser,
        AirportService airportService, CountryService countryService, CityService cityService, Airport airport)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _currentUser = currentUser;
        
        _airportService = airportService;
        _countryService = countryService;
        _cityService = cityService;
        
        _airport = airport;
        DataContext = _airport;
        
        AddButton.IsVisible = false;
        SaveButton.IsVisible = true;
        
        Loaded += async (_, _) => await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        await _mainWindow.RunAnim();
        CountriesComboBox.ItemsSource = await _countryService.GetAllAsync();
        CitiesComboBox.ItemsSource = await _cityService.GetAllAsync();
        await _mainWindow.StopAnim();
    }

    private void PrepareForSave()
    {
        if (_airport.Country != null) _airport.CountryId = _airport.Country.Id;
        if (_airport.City != null) _airport.CityId = _airport.City.Id;

        _airport.Country = null!;
        _airport.City = null!;
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_airport.Code)) return;
        try
        {
            await _airportService.UpdateAsync(_airport);
            _mainWindow.ShowSuccessNotification("Аэропорт обновлен");
            BackButton_OnClick(null, e);
        }
        catch
        {
            _mainWindow.ShowErrorNotification("Ошибка обновления");
        }
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_airport.Code)) return;
        try
        {
            PrepareForSave();
            await _airportService.CreateAsync(_airport);
            _mainWindow.ShowSuccessNotification("Аэропорт добавлен");
            BackButton_OnClick(null, e);
        }
        catch
        {
            _mainWindow.ShowErrorNotification("Ошибка создания");
        }
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e) =>
        await _mainWindow.ShowAdmin(_currentUser);
}