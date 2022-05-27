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
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateGenre(Genre genre) => Create(genre);

        public void DeleteGenre(Genre genre) => Delete(genre);

        public async Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(g => g.Books)
            .OrderBy(g => g.Name)
            .ToListAsync();

        public async Task<Genre> GetGenreAsync(int id, bool trackChanges) =>
            await FindByCondition(g => g.Id.Equals(id), trackChanges)
            .Include(g => g.Books)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(g => ids.Contains(g.Id), trackChanges)
            .Include(g => g.Books)
            .ToListAsync();
    }
}
