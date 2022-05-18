using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Contracts.Repositories;
using Entities;
using System.Linq;

namespace Repository.UserClasses
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext)
            :base (repositoryContext)
        {
        }

        public void CreateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

        public Book GetBook(int BookId, bool trackChanges) =>
            FindByCondition(b => b.Id.Equals(BookId), trackChanges)
            .SingleOrDefault();
    }
}
