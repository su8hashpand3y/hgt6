import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { MemoryService } from '../memory.service';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent  {

  email: string="";
  password: string="";
  loading:boolean = false;
  errors:string;
  constructor(private memory:MemoryService, private router:Router, private toast: ToastrService, private http: HttpClient, public dialogRef: MatDialogRef<LoginComponent>,private baseAddress:BaseAddressService ) { }

  signOut() {
      localStorage.removeItem('token');
      this.toast.success("Successfully Logged Out")
  }

  signIn() {
    this.loading=true;
      var user = {
          email: this.email,
          password: this.password
      };  
              this.http.post<IServiceResponse>(this.baseAddress.get()+"/api/Login/Login", user).subscribe(x => {
        
                if(x.status == 'registerd'){
                    localStorage.setItem('token', x.message);
                    this.toast.info(`Thank You.You are logged in as ${user.email}`);
                    console.log("dialog ref");
                    console.log(this.dialogRef)
                    if (this.dialogRef)
                    this.dialogRef.close(x.message);
                   this.router.navigateByUrl('/Home');
                }
                  else
                    this.toast.error(`${this.email} was not Logged In`);
                  
                  if(x.status == 'error'){
                   this.toast.error(x.message);
                  }
                  if (this.dialogRef)
                  this.dialogRef.close();
                  this.loading= false
              });
  }

  cancel(){
    this.dialogRef.close();
    this.router.navigateByUrl('/Home');
  }

  forgotPassword(){
    this.cancel();
    console.log("forgotten");
    //this.router.navigateByUrl('ForgotPassword');
  }

  saveUrl(){
    this.memory.setReturnUrl(this.router.url);
    this.cancel();
  }

}
