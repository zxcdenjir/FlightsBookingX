using System;
using System.Collections.Generic;

namespace FlightsBookingLib.Models;

public partial class Airport
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public int CountryId { get; set; }

    public int CityId { get; set; }
    
    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Flight> FlightFromAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightToAirports { get; set; } = new List<Flight>();
    
    public override string ToString() => $"{Code}, {City.Name}";
}
