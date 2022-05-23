﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObject
{
    public class BookForCreationDto
    {
        public string Name { get; set; }
        public IEnumerable<AuthorForCreationDto> Authors { get; set; }
    }
}
