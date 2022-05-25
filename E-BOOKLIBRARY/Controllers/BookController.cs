using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using E_BOOKLIBRARY.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("books")]
        public IActionResult GetCategories()
        {
            var books = _bookService.Books;

            if (books == null)
            {
                return NotFound("No book found");
            }
            return Ok(books);
        }

        [HttpGet("{Id}")]
        public IActionResult GetCategoryById(string Id)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");
            var book = _bookService.GetBook(Id);
            if (book == null)
            {
                return NotFound("No book found with this Id");
            }
            return Ok(book);
        }

        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] BookDTO bk)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");
            var newBook = new Book()
            {
                Title = bk.Title,
                Description = bk.Description,
                ISBN = bk.ISBN,
                CategoryId = new Category().Id
            };
            _bookService.AddBook(newBook);
            return Ok("Category added successfully");
        }

        [HttpDelete("DeleteCategory/{Id}")]
        public IActionResult DeleteBook(string Id)
        {
            _bookService.DeleteBook(Id);
            return Ok("Category deleted successfully");
        }

        [HttpPut("updatebook/{Id}")]
        public IActionResult UpdateBook(Book book)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");
            var BookToEdit = _bookService.EditBook(book);
            if (BookToEdit == null) return Ok("Book cannot be empty");
            return Ok("Category successfully added");
        }
    }
}
