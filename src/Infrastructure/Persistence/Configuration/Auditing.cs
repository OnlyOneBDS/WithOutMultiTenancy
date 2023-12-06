using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WithOutMultiTenancy.Infrastructure.Auditing;

namespace WithOutMultiTenancy.Infrastructure.Persistence.Configuration;

public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
{
  public void Configure(EntityTypeBuilder<Trail> builder)
  {
    builder
      .ToTable("AuditLogs", SchemaNames.Auditing);
  }
}