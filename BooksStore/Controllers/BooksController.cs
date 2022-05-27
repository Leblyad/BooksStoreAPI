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
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;

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

        [HttpGet("collection/({ids})", Name = "BookCollection")]
        public IActionResult GetBookCollection(IEnumerable<int> ids)
        {
            try
            {
                if(ids == null)
                {
                    _logger.LogError("Parametr is null.");
                    return BadRequest("Parametr ids is null.");
                }

                var bookEntities = _repository.Book.GetBooksByIds(ids, trackChanges: false);

                if(ids.Count() != bookEntities.Count())
                {
                    _logger.LogInfo("Some ids are not valid in collection.");
                    return NotFound();
                }

                var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(bookEntities);

                return Ok(booksToReturn);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetBookCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody]BookForCreationDto book)
        {
            try
            {
                if(book == null)
                {
                    _logger.LogError("BookForCreationDto object sent from client is null.");

                    return BadRequest("BookForCreationDto object in null.");
                }

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the BookForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var bookEntity = _mapper.Map<Book>(book);

                _repository.Book.CreateBook(bookEntity);
                _repository.Save();

                var bookToReturn = _mapper.Map<BookDto>(bookEntity);

                return CreatedAtRoute("BookById", new { id = bookToReturn.Id }, bookToReturn);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateBook)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("collection")]
        public IActionResult CreateBookCollection([FromBody] IEnumerable<BookForCreationDto> bookCollection)
        {
            try
            {
                if(bookCollection == null)
                {
                    _logger.LogError("Book collection sent from client is null.");
                    return BadRequest("Book collection is null");
                }

                var bookEntities = _mapper.Map<IEnumerable<Book>>(bookCollection);
                foreach(var book in bookEntities)
                {
                    _repository.Book.CreateBook(book);
                }

                _repository.Save();

                var bookCollectionToReturn = _mapper.Map<IEnumerable<BookDto>>(bookEntities);
                var ids = string.Join(" ", bookCollectionToReturn.Select(b => b.Id));

                return CreatedAtRoute("BookCollection", new { ids }, bookCollectionToReturn);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateBookCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var book = _repository.Book.GetBook(id, trackChanges: false);

                if(book == null)
                {
                    _logger.LogError($"Book with id: {id} doesn't exist in the database.");
                    return NotFound();
                }

                _repository.Book.DeleteBook(book);
                _repository.Save();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(DeleteBook)} action {ex}");
                return StatusCode(500, "Internal server error.");
            };
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateBook(int id, [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            try
            {
                if(patchDoc == null)
                {

                    return BadRequest();
                }

                var bookEntity = _repository.Book.GetBook(id, trackChanges: true);

                if(bookEntity == null)
                {

                    return NotFound();
                }

                var bookToPatch = _mapper.Map<BookForUpdateDto>(bookEntity);

                patchDoc.ApplyTo(bookToPatch);

                TryValidateModel(bookToPatch);

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the BookForUpdateDto object.");
                    return UnprocessableEntity(ModelState);
                }

                _mapper.Map(bookToPatch, bookEntity);

                MapGenresAndAuthors(bookEntity, bookToPatch);

                _repository.Save();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(PartiallyUpdateBook)} action {ex}");
                return StatusCode(500);
            }
        }

        private void MapGenresAndAuthors(Book bookEntity, BookForUpdateDto bookToPatch)
        {
            var authors = _repository.Author.GetAuthorsByIds(bookToPatch.AuthorsIds, trackChanges: false).ToList();

            bookEntity.Authors = authors;

            var genres = _repository.Genre.GetGenresByIds(bookToPatch.GenresIds, trackChanges: false).ToList();

            bookEntity.Genres = genres;
        }
    }
}
