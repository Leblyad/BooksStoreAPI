using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Entities.DataTransferObject;

using Contracts;

namespace BooksStore
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(a => a.Initials,
                opt => opt.MapFrom(x => string.Join(" ", x.Name[0] + ".", x.Surname[0] + ".")));

            CreateMap<AuthorForCreationDto, Author>();

            CreateMap<AuthorForUpdateDto, Author>().ReverseMap();

            CreateMap<Genre, GenreDto>();

            CreateMap<GenreForCreationDto, Genre>();

            CreateMap<GenreForUpdateDto, Genre>().ReverseMap();

            CreateMap<Book, BookDto>();

            CreateMap<BookForCreationDto, Book>();

            CreateMap<Book, BookForUpdateDto>();

            CreateMap<BookForUpdateDto, Book>().ReverseMap();

            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
