using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FlightsBookingLib.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле")]
    public string SecondName { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле")]
    public string MiddleName { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле")]
    [Phone(ErrorMessage = "Неверный формат")]
    public string PhoneNumber { get; set; } = null!;
    
    [Required(ErrorMessage = "Заполните поле")]
    public string PassportData { get; set; } = null!;
    
    public DateOnly DateOfBirth { get; set; }
    
    
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Заполните поле")]
    [EmailAddress(ErrorMessage = "Неверный формат")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Заполните поле")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Заполните поле")]
    public virtual UserRole Role { get; set; } = null!;

    public virtual ObservableCollection<Ticket> Tickets { get; set; } = new();
    
    [Required(ErrorMessage = "Заполните поле")]
    public virtual DateTime DateTimeOfBirth { get => DateOfBirth.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc); set => DateOfBirth = DateOnly.FromDateTime(value); }

    public override string ToString() =>  $"{SecondName} {FirstName} {MiddleName}";
}
