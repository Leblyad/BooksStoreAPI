using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObject;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BooksStore.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public AuthorsController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            try
            {
                var authors = _repository.Author.GetAllAuthors(trackChanges: false);

                var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);

                return Ok(authorsDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAuthors)} action {ex}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "AuthorById")]
        public IActionResult GetAuthor(int Id)
        {
            try
            {
                var author = _repository.Author.GetAuthor(Id, trackChanges: false);

                var authorDto = _mapper.Map<AuthorDto>(author);

                if (author == null)
                {
                    _logger.LogInfo($"Author with id: {Id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    return Ok(authorDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetAuthor)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("collection/({ids})", Name = "AuthorCollection")]
        public IActionResult GetAuthorCollection(IEnumerable<int> ids)
        {
            try
            {
                if (ids == null)
                {
                    _logger.LogError("Parametr is null.");
                    return BadRequest("Parametr ids is null.");
                }

                var authorEntities = _repository.Author.GetAuthorsByIds(ids, trackChanges: false);

                if (ids.Count() != authorEntities.Count())
                {
                    _logger.LogInfo("Some ids are not valid in collection.");
                    return NotFound();
                }

                var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

                return Ok(authorsToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetAuthorCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] AuthorForCreationDto author)
        {
            try
            {
                if (author == null)
                {
                    _logger.LogError("AuthorForCreationDto object sent from client is null.");

                    return BadRequest("AuthorForCreationDto object in null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the AuthorForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var authorEntity = _mapper.Map<Author>(author);

                _repository.Author.CreateAuthor(authorEntity);
                _repository.Save();

                var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

                return CreatedAtRoute("AuthorById", new { id = authorToReturn.Id }, authorToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateBook)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("collection")]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            try
            {
                if (authorCollection == null)
                {
                    _logger.LogError("Author collection sent from client is null.");
                    return BadRequest("Author collection is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the AuthorForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);
                foreach (var author in authorEntities)
                {
                    _repository.Author.CreateAuthor(author);
                }

                _repository.Save();

                var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
                var ids = string.Join(" ", authorCollectionToReturn.Select(b => b.Id));

                return CreatedAtRoute("AuthorCollection", new { ids }, authorCollectionToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateAuthorCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            try
            {
                var author = _repository.Author.GetAuthor(id, trackChanges: false);

                if (author == null)
                {
                    _logger.LogError($"Author with id: {id} doesn't exist in the database.");
                    return NotFound();
                }

                _repository.Author.DeleteAuthor(author);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(DeleteAuthor)} action {ex}");
                return StatusCode(500, "Internal server error.");
            };
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateAuthor(int id, [FromBody] JsonPatchDocument<AuthorForUpdateDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {

                    return BadRequest();
                }

                var authorEntity = _repository.Author.GetAuthor(id, trackChanges: true);

                if (authorEntity == null)
                {

                    return NotFound();
                }

                var authorToPatch = _mapper.Map<AuthorForUpdateDto>(authorEntity);

                patchDoc.ApplyTo(authorToPatch);

                TryValidateModel(authorToPatch);

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the AuthorForUpdateDto object.");
                    return UnprocessableEntity(ModelState);
                }

                _mapper.Map(authorToPatch, authorEntity);

                MapBooksForUpdateAuthor(authorEntity, authorToPatch);

                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(PartiallyUpdateAuthor)} action {ex}");
                return StatusCode(500);
            }
        }

        private void MapBooksForUpdateAuthor(Author authorEntity, AuthorForUpdateDto authorToPatch)
        {
            var books = _repository.Book.GetBooksByIds(authorToPatch.BooksIds, trackChanges: false).ToList();

            authorEntity.Books = books;
        }
    }
}
