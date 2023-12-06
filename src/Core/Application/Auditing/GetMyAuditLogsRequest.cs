namespace WithOutMultiTenancy.Application.Auditing;

public class GetMyAuditLogsRequest : IRequest<List<AuditDto>>
{
}

public class GetMyAuditLogsRequestHandler : IRequestHandler<GetMyAuditLogsRequest, List<AuditDto>>
{
  private readonly ICurrentUser _currentUser;
  private readonly IAuditService _auditService;

  public GetMyAuditLogsRequestHandler(ICurrentUser currentUser, IAuditService auditService)
  {
    _currentUser = currentUser;
    _auditService = auditService;
  }

  public Task<List<AuditDto>> Handle(GetMyAuditLogsRequest request, CancellationToken cancellationToken)
  {
    return _auditService.GetUserTrailsAsync(_currentUser.GetUserId());
  }
}