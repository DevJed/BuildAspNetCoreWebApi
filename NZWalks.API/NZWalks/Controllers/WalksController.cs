using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.CustomActionFilters;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        
        // CREATE walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain model 
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            
            await _walkRepository.CreateAsync(walkDomainModel);
            
            // Map Domain model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        
        // GET Walks
        // GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain Models
            var walksDomainModel = await _walkRepository.GetAllAsync();
            
            // Map Domain Model to DTO
            return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
        }
        
        // Get Walk by Id
        // GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound(); //404 response
            }
            
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        
        // UPDATE Walk by Id
        // PUT: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            
            if(walkDomainModel == null)
            {
                return NotFound(); //404 response
            }
            
            // Map Domain model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        
        // DELETE walk by Id
        // DELETE: /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound(); //404 response
            }
            
            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}