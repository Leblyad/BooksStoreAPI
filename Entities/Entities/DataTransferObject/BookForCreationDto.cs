using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class BookForCreationDto
    {
        public string Name { get; set; }
        public string AuthorId { get; set; }
        public string GenreId { get; set; }
    }
}
