using WebAPIAuthors.DTOs.Pagination;

namespace WebAPIAuthors.utilities
{
  public static class IQueryableExtensions
  {
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
    {
      return queryable
        .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsByPage)
        .Take(paginationDTO.RecordsByPage);
    }
  }
}
