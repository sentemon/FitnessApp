export class Result<TResponse> {
  private constructor(
    public response?: TResponse,
    public error?: Error,
    public isSuccess: boolean = false
  ) { }

  public static success<TResponse>(response: TResponse): Result<TResponse> {
    return new Result(response, undefined, true);
  }

  public static failure<TResponse>(error: Error): Result<TResponse> {
    return new Result<TResponse>(undefined, error, false);
  }
}
