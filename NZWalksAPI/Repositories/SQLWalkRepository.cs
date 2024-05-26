using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var deletedModel=await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            
            if (deletedModel == null)
            {
                return null;
            }

            _dbContext.Walks.Remove(deletedModel);
            await _dbContext.SaveChangesAsync();

            return deletedModel;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walkDomainModel=await _dbContext.Walks.
                Include("Difficulty").
                Include("Region").
                FirstOrDefaultAsync(x => x.Id == id);

            return walkDomainModel;
        }

        public async Task<Walk?> UpdateAsync(Guid id,Walk walk)
        {
            var walkDomainModel=await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomainModel == null) 
            {
                return null;
            }

            walkDomainModel.Description= walk.Description;
            walkDomainModel.Name= walk.Name;
            walkDomainModel.WalkImageUrl= walk.WalkImageUrl;
            walkDomainModel.LengthInKm=walk.LengthInKm;
            walkDomainModel.DifficultyId= walk.DifficultyId;
            walkDomainModel.RegionId= walk.RegionId;
            
            await _dbContext.SaveChangesAsync();

            return walkDomainModel; 
        }

    }
}
