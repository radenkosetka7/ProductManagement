using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Requests;

namespace ProductsManagement.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO> AddCategory(CategoryRequest categoryRequest);
        Task<CategoryDTO> GetCategory(Guid id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ProductManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryService(ProductManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> AddCategory(CategoryRequest categoryRequest)
        {
            var category = _mapper.Map<Category>(categoryRequest);
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<List<CategoryDTO>> GetAll()
        {
            return _mapper.Map<List<CategoryDTO>>(await _dbContext.Categories.ToListAsync());
        }

        public async Task<CategoryDTO> GetCategory(Guid id)
        {
            return (await _dbContext.Categories.FindAsync(id)) is var category ?
                 _mapper.Map<CategoryDTO>(category) :
                 null;
        }
    }
}
