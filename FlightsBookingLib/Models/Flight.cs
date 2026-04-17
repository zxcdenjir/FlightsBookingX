using System;
using System.Collections.Generic;

namespace FlightsBookingLib.Models;

public partial class Flight
{
    public int Id { get; set; }

    public int AircraftId { get; set; }

    public int FromAirportId { get; set; }

    public int ToAirportId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }
    
    public string FlightNumber { get; set; } = null!;
    
    public decimal BasePrice { get; set; }
    
    public TimeSpan OnTheWay => ArrivalTime - DepartureTime;

    public virtual Aircraft Aircraft { get; set; } = null!;

    public virtual Airport FromAirport { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Airport ToAirport { get; set; } = null!;

    public override string ToString() => $"{FromAirport} -> {ToAirport} ({FlightNumber})";
}
