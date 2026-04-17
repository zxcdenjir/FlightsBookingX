using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using MsBox.Avalonia.Enums;

namespace FlightsBookingX.UserControls.MenuControls;

public partial class ShowUserTicketsControl : UserControl
{
    private readonly MainView _mainWindow = null!;
    private User _currentUser = null!;
    private TicketService _ticketService = null!;

    private ObservableCollection<Ticket> _tickets = null!;

    public ShowUserTicketsControl()
    {
        InitializeComponent();
    }

    public ShowUserTicketsControl(MainView mainWindow, User user)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _currentUser = user;

        Loaded += async (_, _) => await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _ticketService = new TicketService();

        _tickets = _currentUser.Tickets;

        TicketsControl.ItemsSource = _tickets;

        await _mainWindow.StopAnim();
    }

    private async void BackButton_OnClick(object? sender, RoutedEventArgs e) =>
        await _mainWindow.ShowMenu(_currentUser);

    private async void DeleteTicketButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var ticket = ((sender as Button)!.DataContext as Ticket)!;
        var result = await MessageBox.ShowQuestion(_mainWindow, "Вы уверены, что хотите отменить бронь?");
        if (result == ButtonResult.Yes)
        {
            await _mainWindow.RunAnim();
            try
            {
                await _ticketService.DeleteAsync(ticket.Id);
                _tickets.Remove(ticket);
                await _mainWindow.StopAnim();
                _mainWindow.ShowSuccessNotification("Бронь успешно отменена!");
            }
            catch
            {
                await _mainWindow.StopAnim();
                _mainWindow.ShowErrorNotification("Ошибка. Повторите попытку позже.");
            }
        }
    }
}