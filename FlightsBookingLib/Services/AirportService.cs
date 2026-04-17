using System;
using System.Collections.Generic;
using System.Text;
using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightsBookingLib.Services
{
    public class AirportService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
        }

        public async Task<Airport?> GetByIdAsync(int id)
        {
            return await _context.Airports
                .Include(a => a.Country)
                .Include(a => a.City)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Airport>> GetAllAsync()
        {
            return await _context.Airports
                .Include(a => a.Country)
                .Include(a => a.City)
                .ToListAsync();
        }

        public async Task UpdateAsync(Airport airport)
        {
            _context.Airports.Update(airport);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            if (airport == null) return;

            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
        }
    }
}