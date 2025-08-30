import { Injectable } from '@angular/core';
import { PlatformService } from './platform.service';
import { Preferences } from '@capacitor/preferences';
import {Result} from "../types/result/result.type";

@Injectable({ providedIn: 'root' })
export class StorageService {
  private accessToken: string | null = null;
  private refreshToken: string | null = null;
  private userId: string | null = null;
  private username: string | null = null;

  private readonly KEYS = {
    accessToken: "token",
    refreshToken: "refreshToken",
    userId: "userId",
    username: "username"
  };

  constructor(private platform: PlatformService) {}

  public async init(): Promise<void> {
    const accessTokenResult = await this.get(this.KEYS.accessToken);
    const refreshTokenResult = await this.get(this.KEYS.refreshToken);
    const userIdResult = await this.get(this.KEYS.userId);
    const usernameResult = await this.get(this.KEYS.username);

    this.accessToken = accessTokenResult.isSuccess ? accessTokenResult.response : null;
    this.refreshToken = refreshTokenResult.isSuccess ? refreshTokenResult.response : null;
    this.userId = userIdResult.isSuccess ? userIdResult.response : null;
    this.username = usernameResult.isSuccess ? usernameResult.response : null;

  }

  public getAccessToken(): Result<string> { return this.accessToken ? Result.success(this.accessToken) : Result.failure(new Error("Access Token not found")); };
  public getRefreshToken(): Result<string> { return this.refreshToken ? Result.success(this.refreshToken) : Result.failure(new Error("Refresh Token not found")); };
  public getUserId(): Result<string> { return this.userId ? Result.success(this.userId) : Result.failure(new Error("User Id not found")); };
  public getUsername(): Result<string> { return this.username ? Result.success(this.username) : Result.failure(new Error("Username not found")); };

  private async get(key: string): Promise<Result<string>> {
    if (this.platform.isWeb()) {
      const cookie = this.getCookie(key);

      if (cookie) {
        return Result.success(cookie);
      } else {
        return Result.failure(new Error(`Could not find ${key}`));
      }
    } else {
      const { value } = await Preferences.get({ key });

      if (value) {
        return Result.success(value);
      } else {
        return Result.failure(new Error(`Could not find ${key}`));
      }
    }
  }

  public async set(key: string, value: string, days?: number): Promise<void> {
    if (this.platform.isWeb()) {
      this.setCookie(key, value, days);
    } else {
      await Preferences.set({ key, value });
    }

    switch (key) {
      case this.KEYS.accessToken:
        this.accessToken = value;
        break;
      case this.KEYS.refreshToken:
        this.refreshToken = value;
        break;
      case this.KEYS.userId:
        this.userId = value;
        break;
      case this.KEYS.username:
        this.username = value;
        break;
    }
  }

  public async delete(key: string): Promise<void> {
    if (this.platform.isWeb()) {
      this.deleteCookie(key);
    } else {
      await Preferences.remove({ key });
    }
  }

  private getCookie(key: string): string | null {
    const value = document.cookie
      .split('; ')
      .find(row => row.startsWith(`${key}=`))
      ?.split('=')[1];
    return value ?? null;
  }

  private setCookie(key: string, value: string, days?: number): void {
    let expires = '';
    if (days) {
      const date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = `; expires=${date.toUTCString()}`;
    }
    document.cookie = `${key}=${value}${expires}; path=/`;
  }

  private deleteCookie(key: string): void {
    document.cookie = `${key}=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/`;
  }
}
