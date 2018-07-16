
import { ErrorHandler, Injectable, Injector} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
   const toast = this.injector.get(ToastrService);
   if (error instanceof HttpErrorResponse) {
      // Server or connection error happened
      if (!navigator.onLine) {
        toast.error("No Internet Connection found");
        // Handle offline error
        // return notificationService.notify('No Internet Connection');
      } else {
        if(error.status === 401 || error.status == 403)
        {
          localStorage.removeItem('token');
          toast.error("Login again");
          router.navigateByUrl('Login');
        }
        //return notificationService.notify(`${error.status} - ${error.message}`);
      }
   } else {
     // Handle Client Error (Angular Error, ReferenceError...)     
     console.log("Something went wrong with Web Browser :(");
     console.log(error);
   }
     
    console.log("Something went wrong :(");
    console.log(error);

  }
}
