using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Contracts.Repositories;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repository.UserClasses
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateAuthor(Author author) => Create(author);

        public void DeleteAuthor(Author author) => Delete(author);

        public async Task<IEnumerable<Author>> GetAllAuthors(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(a => a.Books)
            .OrderBy(a => a.Surname)
            .ToListAsync();

        public async Task<Author> GetAuthor(int AuthorId, bool trackChanges) =>
            await FindByCondition(a => a.Id.Equals(AuthorId), trackChanges)
            .Include(a => a.Books)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Author>> GetAuthorsByIds(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(a => ids.Contains(a.Id), trackChanges)
            .Include(a => a.Books)
            .ToListAsync();
    }
}
