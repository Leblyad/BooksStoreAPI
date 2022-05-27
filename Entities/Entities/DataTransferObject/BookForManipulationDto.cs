using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObject
{
    public abstract class BookForManipulationDto
    {
        [Required(ErrorMessage = "Book name is required field")]
        [MaxLength(200, ErrorMessage = "Maximum lenght for name is 200 characters.")]
        public string Name { get; set; }
    }
}
