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
}
