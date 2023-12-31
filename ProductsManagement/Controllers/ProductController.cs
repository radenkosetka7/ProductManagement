﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Exceptions;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;
using ProductsManagement.Services;

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
            try
            {
                var productDTO = await _service.AddProduct(productRequest, User);
                if (productDTO == null)
                {
                    ModelState.AddModelError("Type", "Invalid Unit");
                    return BadRequest(ModelState);
                }
                return CreatedAtAction(nameof(GetProduct), new { id = productDTO.Id }, productDTO);
            }
            catch(DbUpdateException ex)
            {
                return Conflict("Product with given code already exists!");
            }
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
                return await GetProduct(productDTO.Id);
            }
            catch (Exceptions.ValidationException ex)
            {
                if (ex.StatusCode == 404)
                {
                    return NotFound();
                }
                else if (ex.StatusCode == 403)
                {
                    return StatusCode(403, "Action forbidden");
                }
                else if (ex.StatusCode == 400)
                {
                    return BadRequest();
                }
                else
                {
                    return StatusCode(500, ex.Message);
                }
            }
            catch (DbUpdateException ex)
            {
                return Conflict("Product with given code already exists!");
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
            catch (Exceptions.ValidationException ex)
            {
                if (ex.StatusCode == 404)
                {
                    return NotFound();
                }
                else if (ex.StatusCode == 403)
                {
                    return StatusCode(403, "Action forbidden");
                }
                else if (ex.StatusCode == 400)
                {
                    return BadRequest();
                }
                else
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpPost("filter")]
        public async Task<ActionResult<List<ProductDTO>>> FilterProducts(FilterRequest filterRequest)
        {
            return Ok(await _service.FilterProducts(filterRequest));
        }
    }
}
