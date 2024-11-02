using System.Net;
using Shared.Application.Abstractions;

namespace Shared.Application.Common;

public class Result<TResponse> : IResult<TResponse, Error>
{

    public TResponse? Response { get; }
    public Error? Error { get; }
    public bool IsSuccess { get; }
    public HttpStatusCode StatusCode { get; }

    private Result(TResponse? response)
    {
        Response = response;
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
    }

    private Result(Error? error)
    {
        Error = error;
        IsSuccess = false;
        StatusCode = HttpStatusCode.BadRequest;
    }
    
    public static Result<TResponse> Success(TResponse response)
    {
        return new Result<TResponse>(response);
    }
    
    public static Result<TResponse> Failure(Error error)
    {
        return new Result<TResponse>(error);
    }
}
