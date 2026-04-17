using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightsBookingLib.Services
{
    public class UserRoleService
    {
        private readonly FlightsBookingContext _context = new();

        public async Task CreateAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRole?> GetByIdAsync(int id)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<UserRole>> GetAllAsync()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task UpdateAsync(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null) return;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
