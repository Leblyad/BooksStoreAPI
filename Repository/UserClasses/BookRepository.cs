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

        public void CreateBook(Book book) => Create(book);

        public IEnumerable<Book> GetAllBooks(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

        public Book GetBook(int BookId, bool trackChanges) =>
            FindByCondition(b => b.Id.Equals(BookId), trackChanges)
            .SingleOrDefault();

        public IEnumerable<Book> GetBooksByIds(IEnumerable<int> ids, bool trackChanges) =>
            FindByCondition(b => ids.Contains(b.Id), trackChanges)
            .ToList();

        public void DeleteBook(Book book) => Delete(book);
    }
}
