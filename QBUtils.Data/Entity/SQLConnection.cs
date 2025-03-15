using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace QBUtils.Data.Entity;

internal class SQLConnection
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool IsDefault { get; set; }

    public required string UserId { get; set; }

    public AppUser? User { get; set; }

    public required string QBFile { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public string ConnectionString()
    {
        var builder = new StringBuilder();

        var dsnFile = QBFile.EndsWith(".dsn", StringComparison.OrdinalIgnoreCase) ? QBFile : $"{QBFile}.dsn";

        using (System.IO.StreamReader reader = new System.IO.StreamReader(dsnFile))
        {
            var line = reader.ReadLine();
            while (line != null)
            {
                if (!line.StartsWith("/") && !line.StartsWith("["))
                {
                    builder.Append($"{line};");
                }

                line = reader.ReadLine();
            }
        }
        builder.Append($"UID={UserName};PWD={Password};");

        return builder.ToString();
    }
}

internal class SQLConnectionConfiguration : IEntityTypeConfiguration<SQLConnection>
{
    public void Configure(EntityTypeBuilder<SQLConnection> builder)
    {
        builder.ToTable(nameof(SQLConnection));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(256);

        builder.Property(p => p.QBFile)
            .IsRequired(true);

        builder.Property(p => p.UserId)
            .IsRequired(true)
            .HasMaxLength(64);

        builder.Property(p => p.Password)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}
