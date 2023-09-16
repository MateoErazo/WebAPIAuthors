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

      CreateMap<Author, AuthorWithBooksDTO>()
        .ForMember(x=>x.Books, options=> options.MapFrom(MapAuthorToAuthorWithBooksDTO));

      CreateMap<Book, BookWithAuthorsDTO>()
        .ForMember(x=>x.Authors, options=>options.MapFrom(MapBookToBookWithAuthors));

      CreateMap<Author, AuthorPatchDTO>().ReverseMap();
    }

    private List<AuthorGetDTO> MapBookToBookWithAuthors(Book book, BookWithAuthorsDTO bookWithAuthorsDTO)
    {
      List<AuthorGetDTO> result = new List<AuthorGetDTO>();

      if (book.AuthorBooks is null)
      {
        return result;
      }

      foreach (AuthorBook authorBook in book.AuthorBooks)
      {
        result.Add(new AuthorGetDTO 
        { 
          Id = authorBook.AuthorId,
          Name = authorBook.Author.Name,
          Age = authorBook.Author.Age,
        });
      }

      return result;
    }

    private List<BookGetDTO> MapAuthorToAuthorWithBooksDTO(Author author, AuthorWithBooksDTO authorWithBooksDTO)
    {
      List<BookGetDTO> result = new List<BookGetDTO>();

      if (author.AuthorBooks is null)
      {
        return result;
      }

      foreach (AuthorBook authorBook in author.AuthorBooks)
      {
        result.Add(new BookGetDTO 
        {
          Id=authorBook.BookId,
          Title = authorBook.Book.Title,
          Description = authorBook.Book.Description,
        });
      }

      return result;
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
