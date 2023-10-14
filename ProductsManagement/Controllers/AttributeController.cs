using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;

namespace ProductsManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ProductManagementDbContext _dbContext;

        public AttributeController(ProductManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttributeDTO>>> GetAll()
        {
            var attributes = await _dbContext.Attributes.ToListAsync();

            return Ok(_mapper.Map<List<AttributeDTO>>(attributes));

        }

        [HttpPost]
        public async Task<ActionResult<AttributeDTO>> AddAttribute(AttributeRequest attributeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(AttributeType), attributeRequest.Type))
            {
                ModelState.AddModelError("Type", "Invalid AttributeType");
                return BadRequest(ModelState);
            }


            var attribute = _mapper.Map<Models.Entities.Attribute>(attributeRequest);
            await _dbContext.Attributes.AddAsync(attribute);
            await _dbContext.SaveChangesAsync();
            var attributeDTO = _mapper.Map<AttributeDTO>(attribute);
            return CreatedAtAction(nameof(GetAttribute), new { id = attributeDTO.Id }, attributeDTO);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttributeDTO>> GetAttribute(Guid id)
        {
            var attribute = await _dbContext.Attributes.FindAsync(id);
            if (attribute == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AttributeDTO>(attribute));

        }
    }
}
