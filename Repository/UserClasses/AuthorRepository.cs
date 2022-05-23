using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Contracts.Repositories;
using Entities;
using System.Linq;

namespace Repository.UserClasses
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateAuthor(Author author) => Create(author);

        public IEnumerable<Author> GetAllAuthors(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(a => a.Surname)
            .ToList();

        public Author GetAuthor(int AuthorId, bool trackChanges) =>
            FindByCondition(a => a.Id.Equals(AuthorId), trackChanges)
            .SingleOrDefault();
    }
}
