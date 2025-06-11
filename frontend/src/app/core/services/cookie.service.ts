import { Injectable } from '@angular/core';
import {Result} from "../types/result/result.type";
import {CustomError} from "../types/result/custom-error.type";

@Injectable({
  providedIn: 'root'
})
export class CookieService {

  constructor() { }

  get(key: string): Result<string> {
    let value = document.cookie
      .split('; ')
      .find(row => row.startsWith(`${key}=`))
      ?.split('=')[1];

    if (!value) {
      return Result.failure(new CustomError(`There is no cookie with key ${key}.`));
    }

    return Result.success(value);
  }

  set(key: string, value: string, days?: number): void {
    let expires = '';
    if (days) {
      const date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = `; expires=${date.toUTCString()}`;
    }

    document.cookie = `${key}=${value || ''}${expires}; path=/`;
  }
}
