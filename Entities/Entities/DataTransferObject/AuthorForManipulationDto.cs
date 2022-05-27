using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObject
{
    public abstract class AuthorForManipulationDto
    {
        [Required(ErrorMessage = "Author Name is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum lenght for the Name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Author Surname is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum lenght for the Surname is 30 characters.")]
        public string Surname { get; set; }

        public string Fathername { get; set; }
    }
}
