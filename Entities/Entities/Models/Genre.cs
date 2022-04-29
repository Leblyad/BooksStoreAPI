using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Genre
    {
        [Column("GenreId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Genre name is required field")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
