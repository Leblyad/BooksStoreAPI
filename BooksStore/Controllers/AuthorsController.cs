using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObject;
using AutoMapper;

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
    }
}
