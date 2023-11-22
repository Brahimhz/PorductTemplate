import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const myToken = '/* get the token from somewhere, possibly using AuthService */';
  console.log(myToken);

  if (myToken) {
    req = req.clone({
      setHeaders: { Authorization: 'Bearer ' + myToken }
    });
  }

  return next(req);
};



import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import {
HttpRequest, HttpHandler,
HttpEvent,
HttpInterceptor
} from '@angular/common/http';

import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent <unknown>>
  {
    const myToken = this.auth.getToken();
    console.log(myToken);

    if(myToken)
      request = request.clone({
        setHeaders: {Authorization: 'Berear ' + 'myToken'}
    })

    return next.handle(request);
  }
}

