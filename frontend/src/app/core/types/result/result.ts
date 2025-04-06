import { CustomError } from "./custom-error";

type SuccessResult<T> = {
  isSuccess: true;
  response: T;
  error?: undefined;
};

type FailureResult = {
  isSuccess: false;
  response?: undefined;
  error: CustomError;
};

export type Result<T> = SuccessResult<T> | FailureResult

export namespace Result {
  export function success<T>(response: T): Result<T> {
    return { isSuccess: true, response };
  }

  export function failure<T>(error: CustomError): Result<T> {
    return { isSuccess: false, error };
  }
}
