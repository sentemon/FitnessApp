import { Injectable } from '@angular/core';
import { Result } from "../types/result/result.type";
import { CustomError } from "../types/result/custom-error.type";
import { PlatformService } from "./platform.service";
import { Preferences } from '@capacitor/preferences';

@Injectable({
  providedIn: 'root'
})
export class CookieService {
  constructor(private platformService: PlatformService) { }

  async get(key: string): Promise<Result<string>> {
    if (this.platformService.isWeb()) {
      let value = document.cookie
        .split('; ')
        .find(row => row.startsWith(`${key}=`))
        ?.split('=')[1];

      if (!value) {
        return Result.failure(new CustomError(`There is no cookie with key ${key}.`));
      }

      return Result.success(value);
    } else {
      const { value } = await Preferences.get({ key });
      if (!value) {
        return Result.failure(new CustomError(`There is no value with key ${key}.`));
      }
      return Result.success(value);
    }
  }

  async set(key: string, value: string, days?: number): Promise<void> {
    if (this.platformService.isWeb()) {
      let expires = '';
      if (days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = `; expires=${date.toUTCString()}`;
      }
      document.cookie = `${key}=${value || ''}${expires}; path=/`;
    } else {
      await Preferences.set({ key, value });
    }
  }

  async delete(key: string): Promise<void> {
    if (this.platformService.isWeb()) {
      document.cookie = `${key}=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/`;
    } else {
      await Preferences.remove({ key });
    }
  }
}
