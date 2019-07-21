using Microsoft.EntityFrameworkCore;
 
namespace Bloggster.Models
{
    public class BlogContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along

        public BlogContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Comment> Comments {get;set;}
        public DbSet<Post> Posts {get;set;}

    }
}