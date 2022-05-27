using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class BookForUpdateDto : BookForManipulationDto
    {
        public IEnumerable<int> AuthorsIds { get;set;}
        public IEnumerable<int> GenresIds { get; set; }
    }
}
    