using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using Contracts;
using Entities.DataTransferObject;
using System.Linq;
using AutoMapper;

namespace BooksStore.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        
        public BooksController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                var books = _repository.Book.GetAllBooks(trackChanges: false);

                var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

                return Ok(booksDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetBooks)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "BookById")]
        public IActionResult GetBook(int Id)
        {
            try
            {
                var book = _repository.Book.GetBook(Id, trackChanges: false);

                var bookDto = _mapper.Map<BookDto>(book);

                if(book == null)
                {
                    _logger.LogInfo($"Book with id: {Id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    return Ok(bookDto);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetBook)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
