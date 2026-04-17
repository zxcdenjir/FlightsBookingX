using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using FlightsBookingX.Views;
using FlightsBookingLib.Models;
using FlightsBookingLib.Services;

namespace FlightsBookingX.UserControls.MenuControls
{
    public class SeatViewModel
    {
        public int Number { get; set; }

        public bool IsOccupied { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSpacer { get; set; }
    }

    //TODO сделать анимацию пролистывания вниз-вверх, если Scrollviewer можно листать
    public partial class SeatSelectionControl : UserControl
    {
        public static readonly StyledProperty<int> CurrentPlaneColumnsProperty =
            AvaloniaProperty.Register<SeatSelectionControl, int>(nameof(CurrentPlaneColumns));

        public int CurrentPlaneColumns
        {
            get => GetValue(CurrentPlaneColumnsProperty);
            set => SetValue(CurrentPlaneColumnsProperty, value);
        }

        public Flight CurrentFlight = null!;
        private MainView _mainWindow = null!;
        private User _currentUser = null!;
        private TicketService _ticketService = null!;

        public SeatSelectionControl()
        {
            InitializeComponent();
            DataContext = this;
            if (Design.IsDesignMode)
            {
                CurrentFlight = new Flight
                {
                    FlightNumber = "TEST-777",
                    Aircraft = new Aircraft { Model = "777", SeatsCount = 160, Manufacturer = "Boeing" },
                    Tickets = new List<Ticket>()
                };
                GenerateMap();
            }
        }

        public SeatSelectionControl(MainView mainWindow, Flight flight, User user)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            CurrentFlight = flight;
            _currentUser = user;
            DataContext = this;
            UpdateFlightInfo();
            GenerateMap();
        }

        private void UpdateFlightInfo()
        {
            FlightRouteBlock.Text = $"{CurrentFlight.FromAirport.City} → {CurrentFlight.ToAirport.City}";
            FlightNumberBlock.Text = $"Рейс: {CurrentFlight.FlightNumber}";
            AircraftModelBlock.Text = CurrentFlight.Aircraft.ToString();
            AircraftModelBlock.Text = $"{CurrentFlight.Aircraft}";

            int total = CurrentFlight.Aircraft?.SeatsCount ?? 0;
            int occupied = CurrentFlight.Tickets?.Count ?? 0;
            int available = total - occupied;

            SeatsInfoBlock.Text = $"Свободно {available} из {total}";
            SeatsProgressBar.Maximum = total;
            SeatsProgressBar.Value = available;
        }

        public void GenerateMap()
        {
            var aircraft = CurrentFlight.Aircraft;

            var config = aircraft.SeatsCount switch
            {
                <= 120 => (Cols: 6, Spacers: new[] { 2 }, TwoDecks: false),
                <= 220 => (Cols: 7, Spacers: new[] { 3 }, TwoDecks: false),
                _ => (Cols: 11, Spacers: new[] { 3, 7 }, TwoDecks: true)
            };

            CurrentPlaneColumns = config.Cols;
            var occupied = CurrentFlight.Tickets?.Select(t => t.SeatNumber).ToList() ?? new List<int>();

            if (config.TwoDecks)
            {
                int half = aircraft.SeatsCount / 2;
                LowerSeatsGrid.ItemsSource = BuildDeck(1, half, config.Cols, config.Spacers, occupied);
                UpperSeatsGrid.ItemsSource =
                    BuildDeck(half + 1, aircraft.SeatsCount, config.Cols, config.Spacers, occupied);
                UpperDeckTab.IsVisible = true;
            }
            else
            {
                LowerSeatsGrid.ItemsSource = BuildDeck(1, aircraft.SeatsCount, config.Cols, config.Spacers, occupied);
                UpperDeckTab.IsVisible = false;
                DeckTabs.BorderThickness = new Thickness(0);
            }
        }

        private List<SeatViewModel> BuildDeck(int startNum, int endNum, int cols, int[] spacers, List<int> occupied)
        {
            var list = new List<SeatViewModel>();
            int current = startNum;

            while (current <= endNum)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (current > endNum) break;

                    if (spacers.Contains(c))
                        list.Add(new SeatViewModel { IsSpacer = true });
                    else
                        list.Add(new SeatViewModel { Number = current++, IsOccupied = occupied.Contains(current - 1) });
                }
            }

            return list;
        }

        private async void BackButton_OnClick(object? sender, RoutedEventArgs e) =>
            await _mainWindow.ShowMenu(_currentUser);
        
        

        private async void BookTicketButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var lowerSeats = (LowerSeatsGrid.ItemsSource as IEnumerable<SeatViewModel>)!;
            var upperSeats = (UpperSeatsGrid.ItemsSource as IEnumerable<SeatViewModel>) ?? new List<SeatViewModel>();

            var selectedSeats = lowerSeats.Concat(upperSeats)
                .Where(s => s.IsSelected && !s.IsOccupied).ToList();

            if (!selectedSeats.Any())
            {
                _mainWindow.ShowWarningNotification("Выберите одно или несколько мест");
                return;
            }

            try
            {
                await _mainWindow.RunAnim();
                _ticketService = new();

                List<Ticket> tickets = [];

                foreach (var seat in selectedSeats)
                {
                    tickets.Add(new Ticket
                    {
                        FlightId = CurrentFlight.Id, UserId = _currentUser.Id, SeatNumber = seat.Number,
                        BookingDate = DateTime.Now
                    });
                    seat.IsSelected = false;
                    seat.IsOccupied = true;
                }

                await _ticketService.CreateRangeAsync(tickets);
                await _mainWindow.StopAnim();
                if (tickets.Count == 1)
                    _mainWindow.ShowSuccessNotification($"Билет на рейс {CurrentFlight} успешно забронирован");
                else
                {
                    _mainWindow.ShowSuccessNotification(
                        $"Успешно забронированных билетов на рейс {CurrentFlight}: {tickets.Count}");
                    _currentUser = (await new UserService().GetByIdAsync(_currentUser.Id))!;
                    await _mainWindow.ShowMenu(_currentUser);
                }
            }
            catch (Exception ex)
            {
                await _mainWindow.StopAnim();
                _mainWindow.ShowErrorNotification(ex.Message);
            }
        }

        
    }
}