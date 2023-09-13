using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.DTOs.Author
{
    public class AuthorGetDTO
    {
        public int Id { get; set; }

        [Required]
        [FirstLetterCapitalized]
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
