namespace WebAPIAuthors.DTOs.HATEOAS
{
  public class ResourceCollection<T>:Resource where T : Resource
  {
    public List<T> Values = new List<T>();
  }
}
