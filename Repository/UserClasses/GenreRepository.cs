using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Contracts.Repositories;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.UserClasses
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public void CreateGenre(Genre genre) => Create(genre);

        public void DeleteGenre(Genre genre) => Delete(genre);

        public IEnumerable<Genre> GetAllGenres(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(g => g.Books)
            .OrderBy(g => g.Name)
            .ToList();

        public Genre GetGenre(int id, bool trackChanges) =>
            FindByCondition(g => g.Id.Equals(id), trackChanges)
            .Include(g => g.Books)
            .SingleOrDefault();

        public IEnumerable<Genre> GetGenresByIds(IEnumerable<int> ids, bool trackChanges) =>
            FindByCondition(g => ids.Contains(g.Id), trackChanges)
           .Include(g => g.Books)
            .ToList();
    }
}
