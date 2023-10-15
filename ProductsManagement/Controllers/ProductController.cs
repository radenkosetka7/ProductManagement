using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;
using ProductsManagement.Services;
using System.ComponentModel.DataAnnotations;

namespace ProductsManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductRequest productRequest)
        {
            var productDTO = await _service.AddProduct(productRequest,User);
            if (productDTO == null)
            {
                ModelState.AddModelError("Type", "Invalid Unit");
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetProduct), new { id = productDTO.Id }, productDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            return (await _service.GetProduct(id)) is var product ? Ok(product) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(Guid id, ProductRequest productRequest)
        {
            try
            {
                var productDTO = await _service.UpdateProduct(id,productRequest, User);
                return CreatedAtAction(nameof(GetProduct), new { id = productDTO.Id }, productDTO);
            
            }
            catch (ValidationException ex)
            {
                if(ex.Message == "Product not found")
                {
                    return NotFound();
                }
                else if(ex.Message == "Forbidden")
                {
                    return Unauthorized("Action forbidden"); //should be Forbidden 403
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var productDTO = await _service.DeleteProduct(id,User);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                if (ex.Message == "Product not found")
                {
                    return NotFound();
                }
                else if (ex.Message == "Forbidden")
                {
                    return Unauthorized("Action forbidden"); //should be Forbidden 403
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
