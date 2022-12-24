using AutoMapper;
using BookLibrary.Domain.Dto;
using BookLibrary.Domain.Model;

namespace BookLibrary.AutoMapperProfiles
{
    public class BookLibraryProfile: Profile
    { 
        public BookLibraryProfile() 
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
