using LibraryAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using BCrypt.Net;
namespace LibraryAPI.Data.Context;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
        });

        // Book configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ISBN).IsUnique();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
        });

        // Borrowing configuration
        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Book)
                .WithMany(b => b.Borrowings)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Borrowings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
        });



        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@library.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Librarian"
            },
            new User
            {
                Id = 2,
                Username = "john",
                Email = "john@library.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Client@123"),
                Role = "Client"
            }
        );

        modelBuilder.Entity<Book>().HasData(
    new Book
    {
        Id = 1,
        Title = "Clean Code",
        Author = "Robert C. Martin",
        ISBN = "9780132350884",
        TotalCopies = 5,
        AvailableCopies = 5
    },
    new Book
    {
        Id = 2,
        Title = "Design Patterns",
        Author = "GoF",
        ISBN = "9780201633610",
        TotalCopies = 3,
        AvailableCopies = 3
    }
);


    }
}
