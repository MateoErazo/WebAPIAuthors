using AutoMapper;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.DTOs.Book;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors.utilities
{
    public class AutoMapperProfiles:Profile
  {
    public AutoMapperProfiles() 
    {
      CreateMap<Author, AuthorGetDTO>();

      CreateMap<AuthorCreationDTO, Author>();

      CreateMap<Book, BookGetDTO>();

      CreateMap<BookCreationDTO, Book>()
        .ForMember(x=>x.AuthorBooks,options=>options.MapFrom(MapBookCreationDTOToBook));
    }

    private List<AuthorBook> MapBookCreationDTOToBook(BookCreationDTO bookCreationDTO, Book book)
    {
      List<AuthorBook> result = new List<AuthorBook>();

      if (bookCreationDTO.AuthorsIds is null)
      {
        return result;
      }

      foreach (int authorId in bookCreationDTO.AuthorsIds)
      {
        result.Add(new AuthorBook { AuthorId = authorId});
      }

      return result;
    }
  }
}
