using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData
                (
                new Genre
                {
                    Id = 1,
                    Name = "1 Genre"
                },
                new Genre
                {
                    Id = 2,
                    Name = "2 Genre"
                });
        }
    }
}
