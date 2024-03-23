using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksControllers : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksControllers(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<List<Book>> Get() => await _booksService.GetBooksAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetBookAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateBookAsync(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id.ToString() }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetBookAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            updatedBook.Id = book.Id;
            await _booksService.UpdateBookAsync(id, updatedBook);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetBookAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            await _booksService.RemoveBookAsync(id);

            return NoContent();
        }
    }
}
