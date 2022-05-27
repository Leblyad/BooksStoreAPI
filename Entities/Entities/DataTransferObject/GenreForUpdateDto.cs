using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class GenreForUpdateDto : GenreForManipulationDto
    {
        public IEnumerable<int> BooksIds { get; set; }
    }
}
