import { Injectable } from '@angular/core';
import {CookieService} from "ngx-cookie-service";
import {Token} from "../models/token.model";

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private cookieService: CookieService) { }

  get(): any {
    this.cookieService.get("token");
  }

  set(token?: Token): void {
    if (token) {
      this.cookieService.set("token", token.accessToken, token.expiresIn);
    } else {
      console.error("Token cannot be empty or null.")
    }
  }

  delete(): void {
    this.cookieService.delete("token");
  }
}
