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
using Microsoft.AspNetCore.Authorization;

namespace BooksStore.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/books")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// Gets the list of all books
        /// </summary>
        /// <returns>The books list</returns>
        [HttpGet(Name = "GetBooks"), Authorize]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _repository.Book.GetAllBooksAsync(trackChanges: false);

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
        public async Task<IActionResult> GetBook(int Id)
        {
            try
            {
                var book = await _repository.Book.GetBookAsync(Id, trackChanges: false);

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
        public async Task<IActionResult> GetBookCollection(IEnumerable<int> ids)
        {
            try
            {
                if(ids == null)
                {
                    _logger.LogError("Parametr is null.");
                    return BadRequest("Parametr ids is null.");
                }

                var bookEntities = await _repository.Book.GetBooksByIdsAsync(ids, trackChanges: false);

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

        /// <summary>
        /// Creates a newly created book
        /// </summary>
        /// <param name="book"></param>
        /// <returns>A newly created book</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateBook([FromBody]BookForCreationDto book)
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
                await _repository.SaveAsync();

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
        public async Task<IActionResult> CreateBookCollection([FromBody] IEnumerable<BookForCreationDto> bookCollection)
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

                await _repository.SaveAsync();

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
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _repository.Book.GetBookAsync(id, trackChanges: false);

                if(book == null)
                {
                    _logger.LogError($"Book with id: {id} doesn't exist in the database.");
                    return NotFound();
                }

                _repository.Book.DeleteBook(book);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(DeleteBook)} action {ex}");
                return StatusCode(500, "Internal server error.");
            };
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateBook(int id, [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            try
            {
                if(patchDoc == null)
                {

                    return BadRequest();
                }

                var bookEntity = await _repository.Book.GetBookAsync(id, trackChanges: true);

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

                await MapGenresAndAuthors(bookEntity, bookToPatch);

                await _repository.SaveAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(PartiallyUpdateBook)} action {ex}");
                return StatusCode(500);
            }
        }

        private async Task MapGenresAndAuthors(Book bookEntity, BookForUpdateDto bookToPatch)
        {
            var authors = await _repository.Author.GetAuthorsByIdsAsync(bookToPatch.AuthorsIds, trackChanges: false);

            bookEntity.Authors = authors.ToList();

            var genres = await _repository.Genre.GetGenresByIdsAsync(bookToPatch.GenresIds, trackChanges: false);

            bookEntity.Genres = genres.ToList();
        }
    }
}
