using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Exceptions;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;
using System.Security.Claims;

namespace ProductsManagement.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAll();
        Task<ProductDTO> AddProduct(ProductRequest productRequest,ClaimsPrincipal principal);
        Task<ProductDTO> GetProduct(Guid id);
        Task<ProductDTO> UpdateProduct(Guid id, ProductRequest productRequest, ClaimsPrincipal principal);
        Task<ActionResult> DeleteProduct(Guid id, ClaimsPrincipal principal);
    }
    public class ProductService : IProductService
    {
        private readonly ProductManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(ProductManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDTO> AddProduct(ProductRequest productRequest, ClaimsPrincipal principal)
        {
            if (!Enum.IsDefined(typeof(Unit), productRequest.Unit))
            {
                return null;
            }
            var product = _mapper.Map<Product>(productRequest);
            product.UserId = Guid.Parse(principal.FindFirst("id")?.Value);
            await _dbContext.Products.AddAsync(product);
            foreach (var attributeValue in product.AttributeValues)
            {
                attributeValue.ProductId = product.Id;
                await _dbContext.AttributeValues.AddAsync(attributeValue);
            }
            await _dbContext.SaveChangesAsync();
            return await GetProduct(product.Id);
        }

        public async Task<ActionResult> DeleteProduct(Guid id, ClaimsPrincipal principal)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                throw new ValidationException("Product not found",404);
            }
            if (product.UserId != Guid.Parse(principal.FindFirst("id")?.Value))
            {
                throw new ValidationException("Forbidden",403);
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return null;
        }

        public async Task<List<ProductDTO>> GetAll()
        {
            return _mapper.Map<List<ProductDTO>>(await _dbContext.Products.
                Include(p=>p.Category).
                Include(p=>p.AttributeValues).
                ThenInclude(av=>av.Attribute).
                ToListAsync());
        }

        public async Task<ProductDTO> GetProduct(Guid id)
        {
            var product = await _dbContext.Products.Include(p => p.Category).
               Include(p => p.AttributeValues).
               ThenInclude(av => av.Attribute).
               FirstOrDefaultAsync(p => p.Id == id);
            return product != null ? _mapper.Map<ProductDTO>(product) : null;
        }

        public async Task<ProductDTO> UpdateProduct(Guid id, ProductRequest productRequest, ClaimsPrincipal principal)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new ValidationException("Product not found",404);
            }
            if(product.UserId != Guid.Parse(principal.FindFirst("id")?.Value))
            {
                throw new ValidationException("Forbidden",403);
            }
            if (!Enum.IsDefined(typeof(Unit), productRequest.Unit))
            {
                throw new ValidationException("Invalid unit",400);
            }
            _mapper.Map(productRequest, product);
            foreach (var attributeValue in product.AttributeValues)
            {
                attributeValue.ProductId = product.Id;
                _dbContext.Update(attributeValue);
            }
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
            return await GetProduct(product.Id);


        }
    }
}
