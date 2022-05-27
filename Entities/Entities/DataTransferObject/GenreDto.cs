using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
