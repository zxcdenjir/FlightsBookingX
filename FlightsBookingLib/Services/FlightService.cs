using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class FlightService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<Flight?> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.FromAirport)
                .Include(f => f.ToAirport)
                .Include(f => f.Tickets).ThenInclude(a => a.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Flight>> GetAllAsync()
        {
            return await _context.Flights
                .Include(f => f.Aircraft)
                .Include(f => f.FromAirport).ThenInclude(a => a.Country)
                .Include(f => f.ToAirport).ThenInclude(a => a.Country)
                .Include(f => f.FromAirport).ThenInclude(a => a.City)
                .Include(f => f.ToAirport).ThenInclude(a => a.City)
                .Include(f => f.Tickets).ThenInclude(a => a.User)
                .ToListAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return;

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
}
