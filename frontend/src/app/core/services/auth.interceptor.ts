import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {CookieService} from "./cookie.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private cookieService: CookieService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let result = this.cookieService.get("token");

    if (result.isSuccess) {
      req = req.clone({
        setHeaders: {
          'Authorization': `Bearer ${result.response}`,
        },
      });
    } else {
      console.error(result.error.message);
    }

    return next.handle(req);
  }
}
