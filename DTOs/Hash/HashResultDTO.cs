namespace WebAPIAuthors.DTOs.Hash
{
  public class HashResultDTO
  {
    public string Hash { get; set; }

    public byte[] Salt { get; set; }
  }
}
