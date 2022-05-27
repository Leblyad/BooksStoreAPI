using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthors(bool trackChanges);
        Task<Author> GetAuthor(int AuthorId, bool trackChanges);
        void CreateAuthor(Author author);
        void DeleteAuthor(Author author);
        Task<IEnumerable<Author>> GetAuthorsByIds(IEnumerable<int> ids, bool trackChanges);
    }
}
