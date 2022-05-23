using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Repositories
{
    public interface IGenreRepository
    {
        Genre GetGenre(int id, bool trackChanges);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
        IEnumerable<Genre> GetGenresByIds(IEnumerable<int> ids, bool trackChanges);
        IEnumerable<Genre> GetAllGenres(bool trackChanges);

    }
}
