using System;
using _02_Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace _03_DataAccess.EntityFramework.Contexts
{
    public class Context : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=127.0.0.1;initial catalog=InternshipBookStore;user id=sa;password=MyPass@word;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(book => book.Category)
                .WithMany(category => category.Books)
                .HasForeignKey(book => book.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
                .HasIndex(product => product.Name);

            modelBuilder.Entity<User>()
                .HasMany(w => w.Watchlists)
                .WithOne(w => w.User)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Watchlist>()
                .HasOne(w => w.User)
                .WithMany(w => w.Watchlists)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NotificationUser>()
                .HasKey(k => new { k.NotificationId, k.UserId });


        }
    }
}
