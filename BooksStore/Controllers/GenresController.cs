using AutoMapper;
using Contracts;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksStore.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GenresController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            try
            {
                var genres = await _repository.Genre.GetAllGenresAsync(trackChanges: false);

                var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);

                return Ok(genresDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetGenres)} action {ex}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GenreById")]
        public async Task<IActionResult> GetGenre(int id)
        {
            try
            {
                var gerne = await _repository.Genre.GetGenreAsync(id, trackChanges: false);

                var genreDto = _mapper.Map<GenreDto>(gerne);

                if (gerne == null)
                {
                    _logger.LogInfo($"Genre with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    return Ok(genreDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetGenre)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("collection/({ids})", Name = "GenreCollection")]
        public async Task<IActionResult> GetGenreCollection(IEnumerable<int> ids)
        {
            try
            {
                if (ids == null)
                {
                    _logger.LogError("Parametr is null.");
                    return BadRequest("Parametr ids is null.");
                }

                var genreEntities = await _repository.Genre.GetGenresByIdsAsync(ids, trackChanges: false);

                if (ids.Count() != genreEntities.Count())
                {
                    _logger.LogInfo("Some ids are not valid in collection.");
                    return NotFound();
                }

                var genresToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);

                return Ok(genresToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(GetGenreCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreForCreationDto genre)
        {
            try
            {
                if (genre == null)
                {
                    _logger.LogError("GenreForCreationDto object sent from client is null.");

                    return BadRequest("GenreForCreationDto object in null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the GenreForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var genreEntity = _mapper.Map<Genre>(genre);

                _repository.Genre.CreateGenre(genreEntity);
                await _repository.SaveAsync();

                var genreToReturn = _mapper.Map<GenreDto>(genreEntity);

                return CreatedAtRoute("AuthorById", new { id = genreToReturn.Id }, genreToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateGenre)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateGenreCollection([FromBody] IEnumerable<GenreForCreationDto> genreCollection)
        {
            try
            {
                if (genreCollection == null)
                {
                    _logger.LogError("Genre collection sent from client is null.");
                    return BadRequest("Genre collection is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the GenreForCreationDto object.");
                    return UnprocessableEntity(ModelState);
                }

                var genreEntities = _mapper.Map<IEnumerable<Genre>>(genreCollection);
                foreach (var genre in genreEntities)
                {
                    _repository.Genre.CreateGenre(genre);
                }

                await _repository.SaveAsync();

                var genreCollectionToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);
                var ids = string.Join(" ", genreCollectionToReturn.Select(b => b.Id));

                return CreatedAtRoute("GenreCollection", new { ids }, genreCollectionToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(CreateGenreCollection)} action {ex}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var genre = await _repository.Genre.GetGenreAsync(id, trackChanges: false);

                if (genre == null)
                {
                    _logger.LogError($"Genre with id: {id} doesn't exist in the database.");
                    return NotFound();
                }

                _repository.Genre.DeleteGenre(genre);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(DeleteGenre)} action {ex}");
                return StatusCode(500, "Internal server error.");
            };
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateGenre(int id, [FromBody] JsonPatchDocument<GenreForUpdateDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {

                    return BadRequest();
                }

                var genreEntity = await _repository.Genre.GetGenreAsync(id, trackChanges: true);

                if (genreEntity == null)
                {

                    return NotFound();
                }

                var genreToPatch = _mapper.Map<GenreForUpdateDto>(genreEntity);

                patchDoc.ApplyTo(genreToPatch);

                TryValidateModel(genreToPatch);

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the GenreForUpdateDto object.");
                    return UnprocessableEntity(ModelState);
                }

                _mapper.Map(genreToPatch, genreEntity);

                await MapBooksForUpdateGenre(genreEntity, genreToPatch);

                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in {nameof(PartiallyUpdateGenre)} action {ex}");
                return StatusCode(500);
            }
        }

        private async Task MapBooksForUpdateGenre(Genre genreEntity, GenreForUpdateDto genreToPatch)
        {
            var books = await _repository.Book.GetBooksByIdsAsync(genreToPatch.BooksIds, trackChanges: false);

            genreEntity.Books = books.ToList();
        }
    }
}

