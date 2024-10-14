using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SimpleTwitter.Api.Infrastructure.Entity;
using SimpleTwitter.Api.Validation;

namespace SimpleTwitter.Api.Infrastructure;

public class SimpleTwitterDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    public SimpleTwitterDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .Property(x => x.CreatedDate)
            .HasDefaultValueSql("getutcdate()");
    }
    
    public override int SaveChanges()
    {
        ValidateAnnotations();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ValidateAnnotations();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ValidateAnnotations()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity);

        foreach (var entity in entities)
        {
            var validationErrors = Annotations.Validate(entity).ToList();

            if (validationErrors.Any())
            {
                throw new ValidationException(string.Join(", ", validationErrors));
            }
        }
    }
}