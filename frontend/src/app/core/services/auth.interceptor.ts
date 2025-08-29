import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {StorageService} from "./storage.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private storageService: StorageService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let result = this.storageService.getAccessToken();
    if (result.isSuccess) {
      req = req.clone({
        setHeaders: {
          'Authorization': `Bearer ${result.response}`,
        },
      });
    } else {
      console.log(result.error.message);
    }

    return next.handle(req);
  }
}
