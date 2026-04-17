using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class UserService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(x => x.Role)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.Aircraft)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.FromAirport)
                .ThenInclude(x => x.City)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.FromAirport)
                .ThenInclude(x => x.Country)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.ToAirport)
                .ThenInclude(x => x.City)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.ToAirport)
                .ThenInclude(x => x.Country)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.Aircraft)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.FromAirport)
                .ThenInclude(x => x.City)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.FromAirport)
                .ThenInclude(x => x.Country)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.ToAirport)
                .ThenInclude(x => x.City)
                .Include(x => x.Tickets).ThenInclude(t => t.Flight).ThenInclude(x => x.ToAirport)
                .ThenInclude(x => x.Country)
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)    
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CanConnectAsync()
        {
            return await _context.Database.CanConnectAsync();
        }
    }
}