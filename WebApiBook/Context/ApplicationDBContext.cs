using Microsoft.EntityFrameworkCore;
using WebApiBook.Entities;

namespace WebApiBook.Context
{

    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
    }
}
