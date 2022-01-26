using E_BOOKLIBRARY.Data.Static;
using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using E_BOOKLIBRARY.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _cat;


        public CategoryController(ICategoryService cat)
        {
            _cat = cat;
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            var categories = _cat.Categories;

            if (categories == null)
            {
                return NotFound("No category found");
            }

            return Ok(categories);
        }

        [HttpGet("GetCategory/{Id}")]

        public IActionResult GetCategoryById(string Id)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");

            var category = _cat.GetCategory(Id);

            if (category == null)
            {
                return NotFound("No category found with this Id");
            }

            return Ok(category);
        }

        
        

        [HttpPost("add-category")]

        public IActionResult AddNewCategory([FromBody] CatDTO cat)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");

            var newCat = new Category();
            newCat.Name = cat.Name;

            _cat.AddCategory(newCat);

            return Ok("Category added successfully");
        }

       

        [HttpDelete("DeleteCategory/{Id}")]
        public IActionResult DeleteCategory(string Id)
        {
            _cat.DeleteCategory(Id);
            return Ok("Category deleted successfully");
        }

        

        [HttpPut("updatecategory/{Id}")]

        public IActionResult UpdateCategory(Category cat)
        {
            if (!ModelState.IsValid) return Ok("Invalid details");
            var CatToEdit = _cat.EditCategory(cat);
            if (CatToEdit == null) return Ok("Category cannot be empty");
            return Ok("Category successfully added");
        }

    }
}
