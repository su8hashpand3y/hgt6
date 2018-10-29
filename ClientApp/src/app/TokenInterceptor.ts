import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';
@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  
  intercept(request: HttpRequest<any>, next: HttpHandler):     Observable<HttpEvent<any>> {

    try {
      let token = localStorage.getItem('token');
      token = `Bearer ${token}`;
      request = request.clone({
        setHeaders: {
          Authorization: token
        }
      });
    }
    catch (e) {
    }

    return next.handle(request);
  }
}
