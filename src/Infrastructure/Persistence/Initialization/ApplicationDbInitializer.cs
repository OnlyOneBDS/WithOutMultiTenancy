using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithOutMultiTenancy.Infrastructure.Persistence.Context;

namespace WithOutMultiTenancy.Infrastructure.Persistence.Initialization;

internal class ApplicationDbInitializer
{
  private readonly ApplicationDbContext _context;
  private readonly ApplicationDbSeeder _dbSeeder;
  private readonly ILogger<ApplicationDbInitializer> _logger;

  public ApplicationDbInitializer(ApplicationDbContext context, ApplicationDbSeeder dbSeeder, ILogger<ApplicationDbInitializer> logger)
  {
    _context = context;
    _dbSeeder = dbSeeder;
    _logger = logger;
  }

  public async Task InitializeAsync(CancellationToken cancellationToken)
  {
    if (_context.Database.GetMigrations().Any())
    {
      if ((await _context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
      {
        _logger.LogInformation("Applying migrations...");
        await _context.Database.MigrateAsync(cancellationToken);
      }

      if (await _context.Database.CanConnectAsync(cancellationToken))
      {
        _logger.LogInformation("Connection to database succeeded...");
        await _dbSeeder.SeedDatabaseAsync(_context, cancellationToken);
      }
    }
  }
}
