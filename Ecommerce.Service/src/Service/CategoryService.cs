
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.RepoAbstract;
using AutoMapper;
using Ecommerce.Core.src.Common;
using System.Text.RegularExpressions;

namespace Ecommerce.Service.src.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(IMapper mapper, ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync()
        {
            try
            {
                var Categories = await _categoryRepo.GetAllCategoriesAsync();
                var CategoryReadDtos = Categories.Select(c => _mapper.Map<Category, CategoryReadDto>(c));
                return CategoryReadDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CategoryReadDto> GetCategoryByIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new Exception("bad request");
            }
            try
            {
                // if not found, repo will throw AppException.NotFound here
                var foundCategory = await _categoryRepo.GetCategoryByIdAsync(categoryId);
                var foundCategoryDto = _mapper.Map<Category, CategoryReadDto>(foundCategory);
                return foundCategoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                // validations
                if (string.IsNullOrEmpty(categoryCreateDto.Name)) throw AppException.InvalidInputException("Category name cannot be empty");
                if (categoryCreateDto.Name.Length > 20) throw AppException.InvalidInputException("Category name cannot be longer than 20 characters");

                // string imagePatten = @"^.*\.(jpg|jpeg|png|gif|bmp)$";
                // Regex imageRegex = new(imagePatten);
                // if (categoryCreateDto.Image is not null && !imageRegex.IsMatch(categoryCreateDto.Image)) throw AppException.InvalidInputException("Category image can only be jpg|jpeg|png|gif|bmp");

                var newCategory = _mapper.Map<CategoryCreateDto, Category>(categoryCreateDto);
                var createdCategory = await _categoryRepo.CreateCategoryAsync(newCategory);
                var createdCategoryDto = _mapper.Map<Category, CategoryReadDto>(createdCategory);
                return createdCategoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CategoryReadDto> UpdateCategoryByIdAsync(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                 var foundCategory = await _categoryRepo.GetCategoryByIdAsync(categoryId);

                // validations
                if (categoryUpdateDto.Name is not null && string.IsNullOrEmpty(categoryUpdateDto.Name)) throw AppException.InvalidInputException("Category name cannot be empty");
                if (categoryUpdateDto.Name is not null && categoryUpdateDto.Name.Length > 20) throw AppException.InvalidInputException("Category name cannot be longer than 20 characters");

                // string imagePatten = @"^.*\.(jpg|jpeg|png|gif|bmp)$";
                // Regex imageRegex = new(imagePatten);
                // if (categoryUpdateDto.Image is not null && !imageRegex.IsMatch(categoryUpdateDto.Image)) throw AppException.InvalidInputException("Category image can only be jpg|jpeg|png|gif|bmp");

                foundCategory.Name = categoryUpdateDto.Name ?? foundCategory.Name;
                foundCategory.Image = categoryUpdateDto.Image ?? foundCategory.Image;

                var updateCategory = await _categoryRepo.UpdateCategoryByIdAsync(foundCategory);

                var updatedCategoryDto = _mapper.Map<Category, CategoryReadDto>(updateCategory);

                return updatedCategoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                // if category not found, repo will throw AppException.NotFound
                var deleted = await _categoryRepo.DeleteCategoryByIdAsync(categoryId);

                // Return true to indicate successful deletion
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}