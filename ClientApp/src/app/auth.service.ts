import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from "@angular/material";
import { Subject } from 'rxjs';
import { LoginComponent } from './login/login.component';
import { IServiceResponse } from './ViewModels/IServiceResponse';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private dialog: MatDialog,private toaster:ToastrService) { }

   returnUrl:string;

   getReturnUrl(){
       let url = this.returnUrl;
       this.returnUrl = '';
       return url;
   }

   setReturnUrl(value:string){
       this.returnUrl = value;
   }

  isAuthenticated() {
      let token = localStorage.getItem('token');
      return token;
  }

  login(user: { email: string, password: string }) {
      let finish = new Subject();
      this.http.post<IServiceResponse>("/api/Login/Login", user).subscribe(x => {

        if(x.status == 'registerd'){
            localStorage.setItem('token', x.message);
            this.toaster.info(`Thank You.You are logged in as ${user.email}`);
            finish.next({sucess:{token: x.message}});
          }
          if(x.status == 'error'){
           this.toaster.error(x.message);
          }

         
      });

      return finish;
  }

  logout() {
      localStorage.removeItem('token');
  }

  openLoginDialog() {
      let finish = new Subject();
      let dialogRef = this.dialog.open(LoginComponent, {disableClose: true});

      dialogRef.afterClosed().subscribe((result: any) => {
          if (result && result.token) {

              localStorage.setItem('token', result.token);
              finish.next(result);
          }
          else{
            finish.next();
          }
      });

      return finish;
  }
}
