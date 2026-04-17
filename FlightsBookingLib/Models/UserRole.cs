using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightsBookingLib.Models;

public partial class UserRole
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public override string ToString() => Name;
}
