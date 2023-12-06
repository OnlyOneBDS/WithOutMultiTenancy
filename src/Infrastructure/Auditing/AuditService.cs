using Mapster;
using Microsoft.EntityFrameworkCore;
using WithOutMultiTenancy.Application.Auditing;
using WithOutMultiTenancy.Infrastructure.Persistence.Context;

namespace WithOutMultiTenancy.Infrastructure.Auditing;

public class AuditService : IAuditService
{
  private readonly ApplicationDbContext _context;

  public AuditService(ApplicationDbContext context) => _context = context;

  public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
  {
    var trails = await _context.AuditLogs
      .Where(a => a.UserId == userId)
      .OrderByDescending(a => a.DateTime)
      .Take(250)
      .ToListAsync();

    return trails.Adapt<List<AuditDto>>();
  }
}