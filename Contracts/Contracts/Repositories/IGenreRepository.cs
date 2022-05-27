using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre> GetGenreAsync(int id, bool trackChanges);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges);

    }
}
