namespace MusicHub.Data;

using Microsoft.EntityFrameworkCore;

using Models;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    {
    }

    public MusicHubDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Producer> Producers { get; set; }

    public DbSet<Album> Albums { get; set; }

    public DbSet<Song> Songs { get; set; }

    public DbSet<Performer> Performers { get; set; }

    public DbSet<Writer> Writers { get; set; }

    public DbSet<SongPerformer> SongsPerformers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(Configuration.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Song>(entity =>
        {
            entity
                .Property(s => s.CreatedOn)
                .HasColumnType("date");
        });

        builder.Entity<Album>(entity =>
        {
            entity
                .Property(a => a.ReleaseDate)
                .HasColumnType("date");
        });

        builder.Entity<SongPerformer>(entity =>
        {
            entity
                .HasKey(sp => new { sp.PerformerId, sp.SongId });
        });
    }
}
