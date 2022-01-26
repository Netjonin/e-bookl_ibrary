using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public interface ICategoryService
    {
        public List<Category> Categories { get; }

        Task<bool> AddCategory(Category category);
        Task<Category> GetCategory(string catId);
        Task<Category> EditCategory(Category category);
        Task<bool> DeleteCategory(string categoryId);
    }
}
