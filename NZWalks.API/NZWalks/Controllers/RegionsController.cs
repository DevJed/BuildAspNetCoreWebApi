using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    // https://localhost:1234/api/regions will be pointing at the RegionsController
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NzWalksDbContext dbContext, IRegionRepository regionRepository, 
            IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Models
            var regionsDomain = await _regionRepository.GetAllAsync();
            
            // Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }
        
        // GET SINGLE REGION (Get Region By Id)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get Region Domain Model from Database
            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null) return NotFound();
            
            return Ok(_mapper.Map<RegionDto>(regionDomain));
        }
        
        // POST To create new Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            
            //Use Domain Model to Create Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            
            // Map Domain Model back to DTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            
            
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }
        
        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            
            // Check if region exists
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
        
        // Delete region
        // DELETE : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null) return NotFound();
            
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }
    }    
}

