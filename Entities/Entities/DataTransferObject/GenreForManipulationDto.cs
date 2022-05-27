using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObject
{
    public abstract class GenreForManipulationDto
    {
        [Required(ErrorMessage = "Genre Name is required field.")]
        [MaxLength(30, ErrorMessage = "Maximum lenght for the Name is 30 characters.")]
        public string Name { get; set; }
    }
}
