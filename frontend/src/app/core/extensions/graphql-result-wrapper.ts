import { ApolloError } from '@apollo/client/core';
import { Result } from "../types/result/result";

export async function toResult<T>(promise: Promise<any>): Promise<Result<T>> {
  try {
    const result = await promise;

    if (result.errors.length) {
      const gqlError = result.errors[0];
      return Result.failure<T>(new Error(gqlError.message));
    }

    if (result.data) {
      return Result.success<T>(result.data);
    }

    return Result.failure<T>(new Error('Unknown error'));
  } catch (err: any) {
    if (err instanceof ApolloError) {
      const message = err.message || 'Apollo error';
      return Result.failure<T>(new Error(message));
    }

    return Result.failure<T>(new Error('Unexpected error'));
  }
}
