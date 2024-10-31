using System.Net;
using Shared.Application.Abstractions;

namespace Shared.Application.Common;

public class Result<TResponse> : IResult<TResponse, Error>
{

    public TResponse Response { get; }
    public Error Error { get; }
    public bool IsSuccess { get; }
    public HttpStatusCode StatusCode { get; }

    public Result(TResponse response)
    {
        Response = response;
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
    }

    public Result(Error error)
    {
        Error = error;
        IsSuccess = false;
        StatusCode = HttpStatusCode.BadRequest;
    }
}
