using E_BOOKLIBRARY.Data.Db;
using E_BOOKLIBRARY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Repositories.CategoryRepo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly BookLibraryContext _ctx;

        public CategoryRepo(BookLibraryContext ctx) => _ctx = ctx;


        public Task<bool> Add<T>(T entity)
        {
            _ctx.Add(entity);
            return SaveChanges();
        }

        public Task<bool> Delete<T>(T entity)
        {
           _ctx.Remove(entity);
            return SaveChanges();
        }

        public Task<bool> Edit<T>(T entity)
        {
            _ctx.Update(entity);
            return SaveChanges();
        }

        public async Task<List<Category>> GetCategories()
        {
           return await _ctx.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(string catId)
        {
            return await _ctx.Categories.Include(c => c.Books).FirstAsync(n => n.Id == catId);
        }

        public async Task<int> RowCount()
        {
            return await _ctx.Categories.CountAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0 ;
        }
    }
}
