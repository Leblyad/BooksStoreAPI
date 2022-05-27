using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IBookRepository Book { get; }
        IGenreRepository Genre { get; }
        IAuthorRepository Author { get; }
        Task SaveAsync();
    }
}
