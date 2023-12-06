using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WithOutMultiTenancy.Application.Common.Events;
using WithOutMultiTenancy.Application.Common.Interfaces;
using WithOutMultiTenancy.Domain.Catalog;
using WithOutMultiTenancy.Infrastructure.Persistence.Configuration;

namespace WithOutMultiTenancy.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
  public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
    : base(options, currentUser, serializer, dbSettings, events) { }

  public DbSet<Product> Products => Set<Product>();
  public DbSet<Brand> Brands => Set<Brand>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(SchemaNames.App);
  }
}