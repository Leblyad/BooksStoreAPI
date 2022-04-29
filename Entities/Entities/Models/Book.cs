using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Book
    {
        [Column("BookId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Book name is required field")]
        public string Name { get; set; }
        public List<Author> Authors { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
