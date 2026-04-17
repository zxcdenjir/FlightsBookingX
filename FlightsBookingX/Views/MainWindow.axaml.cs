using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;
using FlightsBookingX.UserControls;
using FlightsBookingX.UserControls.MenuControls;
using FlightsBookingX.UserControls.MenuControls.AdminControls;

namespace FlightsBookingX.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}