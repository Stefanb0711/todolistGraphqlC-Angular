import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import {EMPTY, Observable} from 'rxjs';
import { tap } from 'rxjs/operators';
import {AuthenticationService} from '../services/auth-service.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authServ: AuthenticationService) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if (this.authServ.currentToken !== "") {
      const authenticatedRequest = req.clone({
        headers: req.headers.append("Authorization", "Bearer " + this.authServ.currentToken)
      })
      return next.handle(authenticatedRequest);
    } else if (req.url.toLowerCase().includes("login") ||
    req.url.toLowerCase().includes("register")
    ) {
          return next.handle(req);
    } else {
      return EMPTY;
    }


  }
}
