using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

public partial class UserInteractionsControl : UserControl
{
    private MainView _mainWindow = null!;
    private UserService _userService = null!;
    private UserRoleService _roleService = null!;
    private User _user = null!;
    private User _currentUser = null!;

    public UserInteractionsControl()
    {
        InitializeComponent();
        _roleService = new UserRoleService();
        DataContext = new User
        {
            Id = 0,
            DateTimeOfBirth = DateTime.Now,
            Email = "example@mail.com",
            FirstName = "Имя",
            SecondName = "Фамилия",
            MiddleName = "Отчество",
            PhoneNumber = "+7 900 800 9909",
            PassportData = "4022 020202",
            Role = new UserRole { Id = 1, Name = "Клиент" },
            Password = "password"
        };
        Loaded += async (_, _) => await LoadAsync();
    }

    public UserInteractionsControl(MainView mainWindow, UserService userService, UserRoleService roleService,
        User currentUser)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _userService = userService;
        _currentUser = currentUser;
        _roleService = roleService;
        _user = new User();
        DataContext = _user;
        SaveUserButton.IsVisible = false;
        Loaded += async (_, _) => await LoadAsync();
    }

    public UserInteractionsControl(MainView mainWindow, UserService userService, UserRoleService roleService,
        User currentUser, User user)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _userService = userService;
        _currentUser = currentUser;
        _roleService = roleService;
        _user = user;
        DataContext = _user;
        AddUserButton.IsVisible = false;
        Loaded += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        RolesComboBox.ItemsSource = await _roleService.GetAllAsync();
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.ShowAdmin(_currentUser);
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.RunAnim();
        ValidationContext vContext = new ValidationContext(_user);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(_user, vContext, results, true))
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowErrorNotification("Заполните все поля!");
            return;
        }

        try
        {
            await _userService.UpdateAsync(_user);
            _mainWindow.ShowSuccessNotification($"Пользователь {_user} успешно обновлён!");
        }
        catch (Exception ex)
        {
            await MessageBox.Show(_mainWindow, ex.Message);
            _mainWindow.ShowErrorNotification("Произошла ошибка. Повторите запрос позже.");
        }

        await _mainWindow.RunAnim();
        await _mainWindow.ShowAdmin(_user);
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.RunAnim();
        ValidationContext vContext = new ValidationContext(_user);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(_user, vContext, results, true))
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowWarningNotification("Заполните все поля!");
            return;
        }

        try
        {
            _user.RoleId = _user.Role.Id;
            _user.Role = null!;
            await _userService.CreateAsync(_user);
            _mainWindow.ShowSuccessNotification($"Пользователь {_user} успешно добавлен!");
        }
        catch
        {
            _mainWindow.ShowErrorNotification("Произошла ошибка. Повторите запрос позже.");
        }

        await _mainWindow.ShowAdmin(_user);
    }
}