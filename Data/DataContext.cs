using BookShopApi.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookAuthor> BookAuthor { get; set; }

        public DbSet<BookCategory> BookCategory { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Order> Orsders { get; set; }

        public DbSet<StatusOfOrder> StatusOfOrders { get; set; }

        public DbSet<StatusOfUnit> StatusOfUnits { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<BookAuthor>()
               .HasKey(bc => new { bc.BookId, bc.AuthorId });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(bc => bc.Author)
                .WithMany(c => c.BookAuthors)
                .HasForeignKey(bc => bc.AuthorId);

            modelBuilder.Entity<Book>()
            .Property(b => b.Price).HasColumnType("decimal(6, 2)");
        }
    }
}
