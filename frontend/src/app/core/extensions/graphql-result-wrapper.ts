import { ApolloError } from '@apollo/client/core';
import { Result } from '../types/result/result';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export function toResult<T>(source$: Observable<any>): Observable<Result<T>> {
  return source$.pipe(
    map(result => {
      if (result.errors?.length) {
        const gqlError = result.errors[0];
        return Result.failure<T>(new Error(gqlError.message));
      }

      if (result.data) {
        return Result.success<T>(result.data as T);
      }

      return Result.failure<T>(new Error('Unknown error'));
    }),
    catchError((err: any) => {
      if (err instanceof ApolloError) {
        const message = err.message || 'Apollo error';
        return of(Result.failure<T>(new Error(message)));
      }

      return of(Result.failure<T>(new Error('Unexpected error')));
    })
  );
}
