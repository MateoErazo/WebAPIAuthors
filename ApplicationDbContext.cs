using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<AuthorBook>(options => 
      options.HasKey(x=> new {x.AuthorId,x.BookId}));
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
  }
}
