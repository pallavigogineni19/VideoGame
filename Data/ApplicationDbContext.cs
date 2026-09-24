
// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using System;
using VideoGame.Domain.Entities; // Imports your VideoGameModel entity

namespace VideoGame.Data
{
    public class ApplicationDbContext : DbContext
    {
        // The constructor passes connection configuration details down to EF Core
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This maps your C# Model to a database table named 'VideoGames'
        public DbSet<VideoGameModel> VideoGames => Set<VideoGameModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API layout schema modifications
            modelBuilder.Entity<VideoGameModel>(entity =>
            {
                entity.ToTable("VideoGames");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Genre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Publisher).IsRequired().HasMaxLength(100);
            });

            // Seed Initial Verification Records for the Browse Catalogue
            modelBuilder.Entity<VideoGameModel>().HasData(
                new VideoGameModel
                {
                    Id = 1,
                    Title = "The Witcher 3: Wild Hunt",
                    Genre = "RPG",
                    Publisher = "CD Projekt",
                    ReleaseDate = new DateTime(2015, 5, 19),
                    MetacriticScore = 93
                },
                new VideoGameModel
                {
                    Id = 2,
                    Title = "Elden Ring",
                    Genre = "Action RPG",
                    Publisher = "Bandai Namco",
                    ReleaseDate = new DateTime(2022, 2, 25),
                    MetacriticScore = 96
                }
            );
        }
    }
}