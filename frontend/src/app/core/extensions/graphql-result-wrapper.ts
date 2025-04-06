import { ApolloError } from '@apollo/client/core';
import { Result } from '../types/result/result';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CustomError } from "../types/result/custom-error";

export function toResult<T>(key: string) {
  return (source$: Observable<any>): Observable<Result<T>> => {
    return source$.pipe(
      map(result => {
        if (result.errors?.length) {
          const gqlError = result.errors[0];

          return Result.failure<T>(new CustomError(gqlError.message));
        }

        const data = result.data?.[key];
        if (data !== undefined) {
          return Result.success<T>(data);
        }

        return Result.failure<T>(new CustomError('Unknown error'));
      }),
      catchError((err: any) => {
        const message = err instanceof ApolloError ? err.message || 'Apollo error' : 'Unexpected error';

        return of(Result.failure<T>(new CustomError(message)));
      })
    );
  };
}
