
import { ErrorHandler, Injectable, Injector} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
@Injectable()
export class ErrorsHandler implements ErrorHandler {
    constructor(
        // Because the ErrorHandler is created before the providers, we’ll have to use the Injector to get them.
        private injector: Injector,
    ) { }

  handleError(error: Error | HttpErrorResponse) {
     // send it to the server

       
   // const notificationService = this.injector.get(NotificationService);
   const router = this.injector.get(Router);
   if (error instanceof HttpErrorResponse) {
      // Server or connection error happened
      if (!navigator.onLine) {
        // Handle offline error
        // return notificationService.notify('No Internet Connection');
      } else {
        // Handle Http Error (error.status === 403, 404...)
        //return notificationService.notify(`${error.status} - ${error.message}`);
      }
   } else {
     // Handle Client Error (Angular Error, ReferenceError...)     
     router.navigate(['/error'], { queryParams: {error: error} })
   }
     
     console.error('Something Went Wrong: ', error);
  }
}