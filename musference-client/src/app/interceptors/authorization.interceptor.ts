import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const idToken = localStorage.getItem("id_token");
      if(idToken){
        const cloned = request.clone({
          headers: request.headers.set("Authorization",
          `bearer ${idToken}`)
        });
        return next.handle(cloned);
      }
      else{
    return next.handle(request);
      }
  }
}
