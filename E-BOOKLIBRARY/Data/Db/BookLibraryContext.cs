using E_BOOKLIBRARY.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Data.Db
{
    public class BookLibraryContext : IdentityDbContext<AppUser>
    {
        public BookLibraryContext(DbContextOptions<BookLibraryContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }       
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserBook> UserBooks { get; set;  }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserBook>()
                .HasKey(s => new { s.UserId, s.BookId });
            builder.Entity<AppUser>().HasMany(x => x.UserBooks).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.Entity<Book>().HasMany(x => x.UserBooks).WithOne(x => x.Book).HasForeignKey(x => x.BookId);

            builder.Entity<Category>()
            .HasMany(c => c.Books)
            .WithOne(b => b.Category);

        }
    }
}
