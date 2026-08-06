using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories;

public class SQLRegionRepository : IRegionRepository
{
    private NzWalksDbContext _dbContext;
    
    public SQLRegionRepository(NzWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Region>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Region> CreateAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync(); // Also generated new ID for the region Domain Model
        return region;
    }

    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (existingRegion == null)
        {
            return null;
        }
        
        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.RegionImageUrl = region.RegionImageUrl;

        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region?> DeleteAsync(Guid id)
    {
        // Check if region exists
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (existingRegion == null)
        {
            return null;
        }
        
        _dbContext.Regions.Remove(existingRegion);
        await _dbContext.SaveChangesAsync(); 
        return existingRegion;
    }
    
}