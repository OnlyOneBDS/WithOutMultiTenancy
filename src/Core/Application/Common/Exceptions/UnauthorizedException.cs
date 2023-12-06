using System.Net;

namespace WithOutMultiTenancy.Application.Common.Exceptions;

public class UnauthorizedException : CustomException
{
  public UnauthorizedException(string message) : base(message, null, HttpStatusCode.Unauthorized) { }
}