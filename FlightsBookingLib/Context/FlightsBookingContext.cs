using System;
using System.Collections.Generic;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightsBookingLib.Context;

public partial class FlightsBookingContext : DbContext
{
    public FlightsBookingContext()
    {
    }

    public FlightsBookingContext(DbContextOptions<FlightsBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }
    
    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=10.228.48.236; Username=postgres; Database=FlightsBooking; Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Aircrafts_pkey");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Airports_pkey");
            
            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("airports_cities_fk");

            entity.HasOne(d => d.Country).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("Airports_CountryId_fkey");
        });
        
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_cities__pk");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Countries_pkey");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Flights_pkey");

            entity.Ignore(e => e.OnTheWay);
            
            entity.Property(e => e.FlightNumber).HasMaxLength(10);

            entity.Property(e => e.ArrivalTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DepartureTime).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Aircraft).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AircraftId)
                .HasConstraintName("Flights_AircraftId_fkey");

            entity.HasOne(d => d.FromAirport).WithMany(p => p.FlightFromAirports)
                .HasForeignKey(d => d.FromAirportId)
                .HasConstraintName("Flights_FromAirportId_fkey");

            entity.HasOne(d => d.ToAirport).WithMany(p => p.FlightToAirports)
                .HasForeignKey(d => d.ToAirportId)
                .HasConstraintName("Flights_ToAirportId_fkey");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tickets_pkey");

            entity.Property(e => e.BookingDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("Tickets_FlightId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Tickets_UserId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");
            entity.Ignore(e => e.DateTimeOfBirth);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_userroles_fk");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userroles_pk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
