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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var walks= _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering

            if (string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false) 
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks=walks.Where(x=>x.Name.Contains(filterQuery)); 
                }

            }

            // Sorting

            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks=isAscending?walks.OrderBy(x => x.LengthInKm) :walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination

            var skipResults=(pageNumber-1)*pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            
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
