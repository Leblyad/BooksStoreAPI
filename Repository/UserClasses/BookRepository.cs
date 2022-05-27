using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Contracts.Repositories;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repository.UserClasses
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext)
            :base (repositoryContext)
        {
        }

        public void CreateBook(Book book) => Create(book);

        public async Task<IEnumerable<Book>> GetAllBooks(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Book> GetBook(int BookId, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(BookId), trackChanges)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Book>> GetBooksByIds(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(b => ids.Contains(b.Id), trackChanges)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .ToListAsync();

        public void DeleteBook(Book book) => Delete(book);
    }
}
