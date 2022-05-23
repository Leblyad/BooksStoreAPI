using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObject;

namespace BooksStore
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(a => a.Initials,
                opt => opt.MapFrom(x => string.Join(" ", x.Name[0] + ".", x.Surname[0] + ".")));

            CreateMap<Book, BookDto>();

            CreateMap<BookForCreationDto, Book>();

            CreateMap<BookForUpdateDto, Book>().ReverseMap();
        }
    }
}
