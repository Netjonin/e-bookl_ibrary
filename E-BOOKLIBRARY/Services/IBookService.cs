using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public interface IBookService
    {
        
            public List<Book> Books { get; }

            Task<bool> AddBook(Book book);
            Task<Book> GetBook(string bookId);
            Task<Book> EditBook(Book book);
            Task<bool> DeleteBook(string categoryId);
    }
}
