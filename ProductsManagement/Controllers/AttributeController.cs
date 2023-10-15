using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;
using ProductsManagement.Services;

namespace ProductsManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _service;

        public AttributeController(IAttributeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttributeDTO>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult<AttributeDTO>> AddAttribute(AttributeRequest attributeRequest)
        {

            var attributeDTO = await _service.AddAttribute(attributeRequest);
            if (attributeDTO == null)
            {
                ModelState.AddModelError("Type", "Invalid AttributeType");
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetAttribute), new { id = attributeDTO.Id }, attributeDTO);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttributeDTO>> GetAttribute(Guid id)
        {
            return (await _service.GetAttribute(id)) is var attribute ? Ok(attribute) : NotFound();
        }
    }
}
