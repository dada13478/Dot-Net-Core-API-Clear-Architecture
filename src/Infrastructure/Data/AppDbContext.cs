using Dotnet_beginner_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_beginner_api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
}
