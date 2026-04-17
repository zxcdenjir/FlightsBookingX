using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightsBookingLib.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FlightId { get; set; }

    [Required(ErrorMessage = "Заполните поле")] 
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Неверный формат")]
    public int SeatNumber { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public DateTime BookingDate { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public virtual Flight Flight { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле")]
    public virtual User User { get; set; } = null!;
    
    public override string ToString() => 
        $"{Flight.FromAirport} -> {Flight.ToAirport} ({Flight.FlightNumber}). Место - {SeatNumber} | {Flight.DepartureTime:g}";
}
        