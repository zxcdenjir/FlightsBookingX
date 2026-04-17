using FlightsBookingLib.Context;
using FlightsBookingLib.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightsBookingLib.Services;

public class CityService
{
    private readonly FlightsBookingContext _context = new();

    public async Task CreateAsync(City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();
    }

    public async Task<City?> GetByIdAsync(int id)
    {
        return await _context.Cities.Include(c => c.Airports)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context.Cities.Include(c => c.Airports).ToListAsync();
    }

    public async Task UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null) return;

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }
}