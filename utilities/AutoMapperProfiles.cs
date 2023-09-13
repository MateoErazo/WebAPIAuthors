using AutoMapper;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors.utilities
{
    public class AutoMapperProfiles:Profile
  {
    public AutoMapperProfiles() 
    {
      CreateMap<Author, AuthorGetDTO>();    
    }
  }
}
