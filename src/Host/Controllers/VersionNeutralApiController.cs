using Asp.Versioning;

namespace WithOutMultiTenancy.Host.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController { }
