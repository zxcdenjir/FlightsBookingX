using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class TicketService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }
        
        public async Task CreateRangeAsync(List<Ticket> tickets)
        {
            await _context.Tickets.AddRangeAsync(tickets);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Flight)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Flight).ThenInclude(x => x.Aircraft)
                .Include(t => t.Flight).ThenInclude(x => x.FromAirport).ThenInclude(x => x.City)
                .Include(t => t.Flight).ThenInclude(x => x.FromAirport).ThenInclude(x => x.Country)
                .Include(t => t.Flight).ThenInclude(x => x.ToAirport).ThenInclude(x => x.City)
                .Include(t => t.Flight).ThenInclude(x => x.ToAirport).ThenInclude(x => x.Country)
                .ToListAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
