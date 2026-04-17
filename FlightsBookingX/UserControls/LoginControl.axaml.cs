using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using FlightsBookingX.Views;

namespace FlightsBookingX.UserControls;

public partial class LoginControl : UserControl
{
    private readonly WelcomeControl _owner = null!;
    
    private readonly UserService _userService = null!;
    
    public LoginControl()
    {
        InitializeComponent();
    }
    
    public LoginControl(UserService userService, WelcomeControl owner)
    {
        InitializeComponent();
        _owner = owner;
        _userService = userService;

        EmailBox.Text = "admin1@mail.com";
        PasswordBox.Text = "admin";
    }
    
    private async void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        await _owner.MainWindow.RunAnim();

        string? email = EmailBox.Text;
        string? password = PasswordBox.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await MessageBox.ShowWarning(_owner.MainWindow, "Заполните все поля!");
            _owner.MainWindow.IsEnabled = true;
            return;
        }

        List<User> users = await _userService.GetAllAsync();

        User? user = users.FirstOrDefault(x => x.Email == email.Trim() && x.Password == password.Trim());

        if (user == null)
        {
            _owner.MainWindow.IsEnabled = true;
            await MessageBox.ShowError(_owner.MainWindow, "Неверный логин или пароль.");
            return;
        }
        
        await _owner.MainWindow.ShowMenu(user);
    }

    private void RegisterButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _owner.SwitchToRegistrationControl();
    }
}