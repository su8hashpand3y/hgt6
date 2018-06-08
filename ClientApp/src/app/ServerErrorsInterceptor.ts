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
export class ServerErrorsInterceptor implements HttpInterceptor {
  
  intercept(request: HttpRequest<any>, next: HttpHandler):     Observable<HttpEvent<any>> {

    let token = localStorage.getItem('token');
    token = `Bearer ${token}`;
    //if (!request.headers)
    //  request.headers = new HttpHeaders({ 'Authorization': token });
    request.headers.append('Authorization', token);
    // If the call fails, retry until 5 times before throwing an error
    return next.handle(request).pipe(retry(5));
  }
}
