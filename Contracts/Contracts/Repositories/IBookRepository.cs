using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetBook(int BookId, bool trackChanges);
        void CreateBook(Book book);
    }
}
