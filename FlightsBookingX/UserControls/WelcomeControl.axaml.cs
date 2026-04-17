using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;
using FlightsBookingX;
using FlightsBookingX.UserControls;
using FlightsBookingLib.Context;
using FlightsBookingLib.Services;
using FlightsBookingX.Views;

namespace FlightsBookingX.UserControls;

public partial class WelcomeControl : UserControl
{
    public readonly MainView MainWindow = null!;

    private UserService _userService = null!;

    private LoginControl _loginControl = null!;
    private RegistrationControl _registrationControl = null!;

    public WelcomeControl()
    {
        InitializeComponent();
        Loaded += async (_, _) => await LoadAsync();
    }

    public WelcomeControl(MainView mainWindow)
    {
        InitializeComponent();
        MainWindow = mainWindow;
        Loaded += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        await MainWindow.RunAnim();

        _userService = new UserService();

        bool canConnect = await _userService.CanConnectAsync();

        if (!canConnect)
        {
            await MessageBox.ShowError(MainWindow, "Не удалось установить соединение с сервером.\nПовторите попытку позже.");
            return;
        }

        _loginControl = new LoginControl(_userService, this);
        _registrationControl = new RegistrationControl(_userService, this);

        ContentPresenter.Content = _loginControl;

        if (!Design.IsDesignMode)
            await MainWindow.StopAnim();
    }

    public void SwitchToLoginControl()
    {
        ContentPresenter.Content = null;
        ContentPresenter.Content = _loginControl;
    }

    public void SwitchToRegistrationControl()
    {
        ContentPresenter.Content = null;
        ContentPresenter.Content = _registrationControl;
    }
}