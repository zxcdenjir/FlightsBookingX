using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightsBookingLib.Models;

public partial class Aircraft
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public string? Model { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public string? Manufacturer { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public int SeatsCount { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public override string ToString() => $"{Manufacturer} {Model}";
}