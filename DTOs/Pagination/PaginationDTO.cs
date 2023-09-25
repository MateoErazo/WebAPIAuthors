namespace WebAPIAuthors.DTOs.Pagination
{
  public class PaginationDTO
  {
    public int Page { get; set; } = 1;

    private int recordsByPage = 10;

    private readonly int amountMaxByPage = 30;

    public int RecordsByPage
    {
      get 
      { 
        return recordsByPage; 
      }
      set
      {
        recordsByPage = (value > amountMaxByPage)? amountMaxByPage:value;
      }
    }
  }
}
