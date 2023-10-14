using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;

namespace ProductsManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ProductManagementDbContext _dbContext;

        public ProductController(IMapper mapper, ProductManagementDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAll()
        {
            var products = await _dbContext.Products.ToListAsync();
            foreach(var product in products)
            {
                var category = await _dbContext.Categories.FindAsync(product.CategoryId);
                product.Category = category;
                var attributeValues = await _dbContext.AttributeValues
               .Where(av => av.ProductId == product.Id)
               .ToListAsync();
                product.AttributeValues = attributeValues;
                foreach (var value in product.AttributeValues)
                {
                    var attribute = await _dbContext.Attributes.FindAsync(value.AttributeId);
                    value.Attribute = attribute;
                }
            }
            return Ok(_mapper.Map<List<ProductDTO>>(products));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(Unit), productRequest.Unit))
            {
                ModelState.AddModelError("Type", "Invalid Unit");
                return BadRequest(ModelState);
            }
            var product= _mapper.Map<Product>(productRequest);
            string userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            
            if(Guid.TryParse(userIdClaim, out Guid userId))
            {
                product.UserId = userId;
            }
            else
            {
                return BadRequest();
            }
            await _dbContext.Products.AddAsync(product);

            foreach(var attributeValue in product.AttributeValues)
            {
                attributeValue.ProductId = product.Id;
                await _dbContext.AttributeValues.AddAsync(attributeValue);
            }
            await _dbContext.SaveChangesAsync();
            var productDTO = _mapper.Map<ProductDTO>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = productDTO.Id }, productDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            var category = await _dbContext.Categories.FindAsync(product.CategoryId);
            var attributeValues = await _dbContext.AttributeValues
                .Where(av => av.ProductId == product.Id)
                .ToListAsync();

            product.Category = category;
            product.AttributeValues = attributeValues;
            foreach(var value in product.AttributeValues)
            {
                var attribute = await _dbContext.Attributes.FindAsync(value.AttributeId);
                value.Attribute = attribute;
            }
            if (product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDTO>(product));
        }
    }
}
