import { Injectable } from '@angular/core';
import {AuthService} from "./auth.service";
import {HttpEvent, HttpHandler, HttpParams, HttpRequest} from "@angular/common/http";
import {exhaustMap, Observable, take} from "rxjs";

@Injectable()
export class AuthInterceptorService {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    return this.authService.user.pipe(
      take(1),
      exhaustMap(user => {
        if(!user){
          return next.handle(req);
        }

        const skipIntercept = req.headers.has('skip');

        if (skipIntercept) {
          const request = req.clone({
            headers: req.headers.delete('skip')
          });
          return next.handle(request);
        }

        const authToken = user.token!;
        const modifiedReq = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${authToken}`)
        });

        return next.handle(modifiedReq);
      })
    )
  }

}
