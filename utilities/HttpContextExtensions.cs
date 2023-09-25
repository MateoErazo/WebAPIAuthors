using Microsoft.EntityFrameworkCore;

namespace WebAPIAuthors.utilities
{
  public static class HttpContextExtensions
  {
    public async static Task InsertPaginationParametersInHead<T>(this HttpContext httpContext, IQueryable<T> queryable)
    {
      if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

      double amount = await queryable.CountAsync();
      httpContext.Response.Headers.Add("totalRecordsAmount", amount.ToString());
    }
  }
}
