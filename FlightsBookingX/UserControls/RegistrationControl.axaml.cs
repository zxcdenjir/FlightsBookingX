using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Core.Plugins;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using FlightsBookingX.Views;

namespace FlightsBookingX.UserControls;

public partial class RegistrationControl : UserControl
{
    private readonly WelcomeControl _welcomeControl = null!;
    
    private readonly UserService _userService = null!;
    
    public RegistrationControl()
    {
        InitializeComponent();
        DataContext = new User {DateTimeOfBirth = DateTime.UtcNow};
    }
    
    public RegistrationControl(UserService userService, WelcomeControl welcomeControl)
    {
        InitializeComponent();
        DataContext = new User {DateTimeOfBirth = DateTime.UtcNow, RoleId = 1};
        _welcomeControl = welcomeControl;
        _userService = userService;
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _welcomeControl.SwitchToLoginControl();
    }

    private async void RegisterButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _welcomeControl.MainWindow.RunAnim();

        var user = (DataContext as User)!;
        
        ValidationContext vContext = new ValidationContext(user); 
        var results = new List<ValidationResult>();
        
        if (!Validator.TryValidateObject(user, vContext, results, true))
        {
            await _welcomeControl.MainWindow.StopAnim();
            _welcomeControl.MainWindow.ShowErrorNotification("Заполните все поля корректно!");
            return;
        }
        
        try
        {
            if ((await _userService.GetAllAsync()).Any(x => x.Email == user.Email))
            {
                _welcomeControl.MainWindow.IsEnabled = true;
                _welcomeControl.MainWindow.ShowErrorNotification("Эта почта уже занята");
                return;
            }
            
            await _userService.CreateAsync(user);
            _welcomeControl.MainWindow.IsEnabled = true;
            _welcomeControl.MainWindow.ShowSuccessNotification("Успешная регистрация");
            (_welcomeControl as WelcomeControl)!.SwitchToLoginControl();
        }
        catch (Exception ex)
        {
            _welcomeControl.MainWindow.IsEnabled = true;
            _welcomeControl.MainWindow.ShowErrorNotification(ex.InnerException?.Message ?? ex.Message);
        }
        
    }
}