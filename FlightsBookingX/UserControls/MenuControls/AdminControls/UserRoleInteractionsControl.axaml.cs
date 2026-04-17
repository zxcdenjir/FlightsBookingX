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

public partial class UserRoleInteractionsControl : UserControl
{
    private MainView _mainWindow = null!;
    private UserRoleService _roleService = null!;
    private User _currentUser = null!;
    private UserRole _role = null!;

    public UserRoleInteractionsControl()
    {
        InitializeComponent();
        _role = new UserRole { Name = "designer" };
        DataContext = _role;
    }

    public UserRoleInteractionsControl(MainView mainWindow, UserRoleService roleService, User currentUser)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _roleService = roleService;
        _currentUser = currentUser;
        _role = new UserRole();
        DataContext = _role;
        SaveRoleButton.IsVisible = false;
    }

    public UserRoleInteractionsControl(MainView mainWindow, UserRoleService roleService, User currentUser, UserRole role)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _roleService = roleService;
        _currentUser = currentUser;
        _role = role;
        DataContext = _role;
        AddRoleButton.IsVisible = false;
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _mainWindow.ShowAdmin(_currentUser);
    }

    private async void SaveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_role.Name))
        {
            _mainWindow.ShowWarningNotification("Введите название роли!");
            return;
        }

        try
        {
            await _mainWindow.RunAnim();
            await _roleService.UpdateAsync(_role);
            await _mainWindow.StopAnim();
            _mainWindow.ShowSuccessNotification($"Роль успешно обновлена!");
            await _mainWindow.ShowAdmin(_currentUser);
        }
        catch
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowErrorNotification("Ошибка при обновлении роли.");
        }

        BackButton_OnClick(null, e);
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_role.Name))
        {
            _mainWindow.ShowWarningNotification("Введите название роли!");
            return;
        }

        try
        {
            await _mainWindow.RunAnim();
            await _roleService.CreateAsync(_role);
            _mainWindow.ShowSuccessNotification($"Роль успешно добавлена!");
            await _mainWindow.ShowAdmin(_currentUser);
        }
        catch
        {
            await _mainWindow.StopAnim();
            _mainWindow.ShowErrorNotification("Ошибка при создании роли.");
        }
        BackButton_OnClick(null, e);
    }
}