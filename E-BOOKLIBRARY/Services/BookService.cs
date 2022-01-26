using E_BOOKLIBRARY.Data.Repositories.BookRepo;
using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;

        public BookService(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }
        public List<Book> Books => _bookRepo.GetBooks().Result;

        public async Task<bool> AddBook(Book book)
        {
            return await _bookRepo.Add(book);
        }

        public async Task<bool> DeleteBook(string bookId)
        {
            var bookToDelete = await _bookRepo.GetBook(bookId);
            if (bookToDelete == null)
                return false;
            return await _bookRepo.Delete(bookToDelete); ;
        }

        public async Task<Book> EditBook(Book book)
        {
            var bookToEdit = await _bookRepo.GetBook(book.Id);

            bookToEdit.Title = book.Title;

            if (await _bookRepo.Edit(book))
                return bookToEdit;

            else return null;
        }

        public async Task<Book> GetBook(string bookId)
        {
            return await _bookRepo.GetBook(bookId);
        }
       
    }
}
