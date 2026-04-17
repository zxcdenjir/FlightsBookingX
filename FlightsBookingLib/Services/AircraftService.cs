using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class AircraftService
    {
        // TODO: привязать все Service к одному контексту
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(Aircraft aircraft)
        {
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
        }

        public async Task<Aircraft?> GetByIdAsync(int id)
        {
            return await _context.Aircrafts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Aircraft>> GetAllAsync()
        {
            return await _context.Aircrafts.ToListAsync();
        }

        public async Task UpdateAsync(Aircraft aircraft)
        {
            _context.Aircrafts.Update(aircraft);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);
            if (aircraft == null) return;

            _context.Aircrafts.Remove(aircraft);
            await _context.SaveChangesAsync();
        }
    }
}
