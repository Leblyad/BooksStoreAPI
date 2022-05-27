using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
