using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasData
                (
                new Author
                {
                    Id = 1,
                    Name = "1 Name",
                    Surname = "1 Surname"
                },
                new Author
                {
                    Id = 2,
                    Name = "2 Name",
                    Surname = "2 Surname"
                });
        }
    }
}
