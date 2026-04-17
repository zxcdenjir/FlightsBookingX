using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class CountryService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task UpdateAsync(Country country)
        {
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return;

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
    }
}
