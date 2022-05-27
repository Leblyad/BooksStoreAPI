using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Contracts.Repositories;
using Entities;
using Repository.UserClasses;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IBookRepository _bookRepository;
        private IGenreRepository _genreRepository;
        private IAuthorRepository _authorRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IBookRepository Book
        {
            get
            {
                if (_bookRepository == null)
                    _bookRepository = new BookRepository(_repositoryContext);

                return _bookRepository;
            }
        }

        public IGenreRepository Genre
        {
            get
            {
                if (_genreRepository == null)
                    _genreRepository = new GenreRepository(_repositoryContext);

                return _genreRepository;
            }
        }

        public IAuthorRepository Author
        {
            get
            {
                if (_authorRepository == null)
                    _authorRepository = new AuthorRepository(_repositoryContext);

                return _authorRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
