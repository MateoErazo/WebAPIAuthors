using System.ComponentModel.DataAnnotations;

namespace WebAPIAuthors.Validations
{
  public class FirstLetterCapitalizedAttribute:ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if(value == null)
      {
        return ValidationResult.Success;
      }

      string firstLetter = value.ToString()[0].ToString();

      if (firstLetter != firstLetter.ToUpper())
      {
        return new ValidationResult("The first letter must be capitalized.");
      }

      return ValidationResult.Success;

    }
  }
}
