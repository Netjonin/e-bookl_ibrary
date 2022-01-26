using E_BOOKLIBRARY.Data.Db;
using E_BOOKLIBRARY.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Repositories.BookRepo
{
    public class BookRepo : IBookRepo
    {
        private readonly BookLibraryContext _ctx;

        public BookRepo(BookLibraryContext ctx)
        {
            _ctx = ctx;
        }
        public Task<bool> Add<T>(T entity)
        {
            _ctx.AddAsync(entity);
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

        public async Task<Book> GetBook(string bookId)
        {
            return await _ctx.Books.FirstAsync(x => x.Id == bookId);
        }

        public Task<List<Book>> GetBooks()
        {
            return _ctx.Books.ToListAsync();
        }

        public async Task<int> RowCount()
        {
            return await _ctx.Books.CountAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }

}
