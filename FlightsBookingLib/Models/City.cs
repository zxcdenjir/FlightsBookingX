using System;
using System.Collections.Generic;

namespace FlightsBookingLib.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
    
    public override string ToString() => Name;
}
