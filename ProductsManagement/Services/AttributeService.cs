using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Enums;
using ProductsManagement.Models.Requests;
using System.Threading.Tasks;

namespace ProductsManagement.Services
{

    public interface IAttributeService
    {
        Task<List<AttributeDTO>> GetAll();
        Task<AttributeDTO> AddAttribute(AttributeRequest attributeRequest);
        Task<AttributeDTO> GetAttribute(Guid id);
    }
    public class AttributeService : IAttributeService
    {
        private readonly ProductManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public AttributeService(ProductManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<AttributeDTO> AddAttribute(AttributeRequest attributeRequest)
        {
            if(!Enum.IsDefined(typeof(AttributeType), attributeRequest.Type))
            {
                return null;
            }

            var attribute = _mapper.Map<Models.Entities.Attribute>(attributeRequest);
            await _dbContext.Attributes.AddAsync(attribute);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AttributeDTO>(attribute);

        }

        public async Task<List<AttributeDTO>> GetAll()
        {
            return _mapper.Map<List<AttributeDTO>>(await _dbContext.Attributes.ToListAsync());
        }

        public async Task<AttributeDTO> GetAttribute(Guid id)
        {
            return (await _dbContext.Attributes.FindAsync(id)) is var attribute ?
                _mapper.Map<AttributeDTO>(attribute) :
                null;
        }
    }
}
