using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Repositories.BookRepo
{
    public interface IBookRepo : ICRUDRepo
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(string email);
        

    }
}
