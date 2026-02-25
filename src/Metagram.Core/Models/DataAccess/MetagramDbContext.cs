using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Metagram.Models.DataAccess;

public class MetagramDbContext : DbContext
{
    public DbSet<MetagramBot> Clients { get; set; }
    public DbSet<MetagramChat> Chats { get; set; }
    public DbSet<MetagramMessage> Messages { get; set; }
    public DbSet<MetagramUser> Users { get; set; }
    public DbSet<MetagramFile> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: Implement migrations
        PrepareMetagramFile(modelBuilder.Entity<MetagramFile>());
        PrepareMetagramBot(modelBuilder.Entity<MetagramBot>());
        PrepareMetagramUser(modelBuilder.Entity<MetagramUser>());
        PrepareMetagramChat(modelBuilder.Entity<MetagramChat>());
        PrepareMetagramMessage(modelBuilder.Entity<MetagramMessage>());
        base.OnModelCreating(modelBuilder);
    }

    private void PrepareMetagramMessage(EntityTypeBuilder<MetagramMessage> builder)
    {
        builder.Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.Now);
        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);
        builder.Property(x => x.Text)
            .HasMaxLength(128)
            .IsRequired(false);
        builder.Property(x => x.Chat)
            .IsRequired(false);
        builder.Property(x => x.User)
            .IsRequired(false);
        builder.Property(x => x.Avatar)
            .IsRequired(false);

    }

    private void PrepareMetagramUser(EntityTypeBuilder<MetagramUser> builder)
    {
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.LastName)
            .IsRequired(false)
            .HasMaxLength(100);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(70);
        builder
            .HasIndex(x => x.TelegramId);
        builder.Property(x => x.Chat)
            .IsRequired(false);
        builder.Property(x => x.Messages)
            .IsRequired(false);
        builder.Property(x => x.Owner)
            .IsRequired(false);

    }

    private static void PrepareMetagramBot(EntityTypeBuilder<MetagramBot> builder)
    {
        builder
            .Property(x => x.TelegramToken)
            .HasMaxLength(128)
            .IsRequired();
        builder.Property(x => x.TelegramToken)
            .HasMaxLength(128)
            .IsRequired();
        builder
            .HasIndex(x => x.IsCurrentBot);
        
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.LastName)
            .IsRequired(false)
            .HasMaxLength(100);
        
        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(70);
    }

    private static void PrepareMetagramChat(EntityTypeBuilder<MetagramChat> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(128);

        builder.Property(x => x.Avatar)
            .IsRequired(false);

    }

    private static void PrepareMetagramFile(EntityTypeBuilder<MetagramFile> builder)
    {
        builder.Property(x => x.Path)
            .HasMaxLength(500);
        builder.HasIndex(x => x.TelegramId);
    }

}
