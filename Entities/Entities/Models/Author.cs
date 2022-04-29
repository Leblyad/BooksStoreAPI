using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Author
    {
        [Column("AuthorId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name name is required field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname name is required field")]
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public List<Book> Books { get; set; }
    }
}
