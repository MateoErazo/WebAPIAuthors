using System.Security.Cryptography.X509Certificates;

namespace WebAPIAuthors.Entities
{
  public class Book
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int AuthorId { get; set; }
  }
}
