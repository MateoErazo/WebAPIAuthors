using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace WebAPIAuthors.Entities
{
  public class Book:IValidatableObject
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<AuthorBook> AuthorBooks { get; set; }


    /// <summary>
    /// This method is an validation to level of model
    /// and It was build of this way to can validate many items of model.
    /// But is important know that this validations only are applyed if the
    /// validations by attribute have already been validate
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      string firstLetterTitle = Title[0].ToString();

      if(firstLetterTitle != firstLetterTitle.ToUpper())
      {
        yield return new ValidationResult("The first letter must be capitalized",
          new String[] {nameof(Title)});
      }

      string firstLetterDescription = Description[0].ToString();

      if (firstLetterDescription != firstLetterDescription.ToUpper())
      {
        yield return new ValidationResult("The first letter must be capitalized",
          new String[] {nameof(Description)});
      }
    }
  }
}
