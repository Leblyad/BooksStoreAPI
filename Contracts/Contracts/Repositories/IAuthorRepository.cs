using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Repositories
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAllAuthors(bool trackChanges);
        Author GetAuthor(int AuthorId, bool trackChanges);
        void CreateAuthor(Author author);
        void DeleteAuthor(Author author);
        IEnumerable<Author> GetAuthorsByIds(IEnumerable<int> ids, bool trackChanges);
    }
}
