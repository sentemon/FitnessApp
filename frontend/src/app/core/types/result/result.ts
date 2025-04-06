import { Error } from "./error";

type SuccessResult<T> = {
  isSuccess: true;
  response: T;
  error?: undefined;
};

type FailureResult = {
  isSuccess: false;
  response?: undefined;
  error: Error;
};

export type Result<T> = SuccessResult<T> | FailureResult

export namespace Result {
  export function success<T>(response: T): Result<T> {
    return { isSuccess: true, response };
  }

  export function failure<T>(error: Error): Result<T> {
    return { isSuccess: false, error };
  }
}
