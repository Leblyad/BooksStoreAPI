using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStoreAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/books")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class BooksV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public BooksV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repository.Book.GetAllBooksAsync(trackChanges: false);
            return Ok(books);
        }
    }
}
