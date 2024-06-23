using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly NZWalksDbContext _dbcontext;
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(NZWalksDbContext dbContext, IWalkRepository walkRepository, IMapper mapper)
        {
            _dbcontext = dbContext;
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
           var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

           walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);

           var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

           return Ok(walkDto);
        }

        // GET:/api/walks?filterOn=Name&filterQuery=Track&SortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
           [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
           [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=1000)
        {

            var walkDomainModel=await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending??true,pageNumber,pageSize);

            // Create an exception

            throw new Exception("This is a new exception");

            var walkDto=_mapper.Map<List<WalkDto>>(walkDomainModel);
            
            return Ok(walkDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
           var walkDomainModel=await _walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto=_mapper.Map<WalkDto>(walkDomainModel); 

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto=_mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateWalkRequestDto updateWalkRequestDto)
        {
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
            
        }
    }
}
