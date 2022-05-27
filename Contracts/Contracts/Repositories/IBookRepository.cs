using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
        Task<Book> GetBookAsync(int BookId, bool trackChanges);
        void CreateBook(Book book);
        Task<IEnumerable<Book>> GetBooksByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        void DeleteBook(Book book);
    }
}
