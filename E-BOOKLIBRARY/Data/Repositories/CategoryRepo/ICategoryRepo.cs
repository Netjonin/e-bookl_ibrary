using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Repositories.CategoryRepo
{
    public interface ICategoryRepo : ICRUDRepo
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string catId);

    }
}
