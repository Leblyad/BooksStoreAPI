using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class AuthorForUpdateDto : AuthorForManipulationDto
    {
        public IEnumerable<int> BooksIds { get; set; }
    }
}
