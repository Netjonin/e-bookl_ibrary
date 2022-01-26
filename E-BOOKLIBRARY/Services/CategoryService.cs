using E_BOOKLIBRARY.Data.Repositories.CategoryRepo;
using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _cateRepo;

        public CategoryService(ICategoryRepo categoryRepo)
        {
            _cateRepo = categoryRepo;
        }
        public List<Category> Categories => _cateRepo.GetCategories().Result;

        public async Task<bool> AddCategory(Category category)
        {
            return await _cateRepo.Add(category);
        }

        public async Task<bool> DeleteCategory(string categoryId)
        {
            var categoryToDelete = await _cateRepo.GetCategory(categoryId);
            if (categoryToDelete == null)
                return false;
            return await _cateRepo.Delete(categoryToDelete);
        }

        public async Task<Category> EditCategory(Category category)
        {
            var categoryToEdit = await _cateRepo.GetCategory(category.Id);

            categoryToEdit.Name = category.Name;

            if (await _cateRepo.Edit(category))
                return categoryToEdit;

            else return null;
        }

        public async Task<Category> GetCategory(string catId)
        {
            return await _cateRepo.GetCategory(catId);
        }
    }
}
