using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks(bool trackChanges);
        Task<Book> GetBook(int BookId, bool trackChanges);
        void CreateBook(Book book);
        Task<IEnumerable<Book>> GetBooksByIds(IEnumerable<int> ids, bool trackChanges);
        void DeleteBook(Book book);
    }
}
