import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { MatDialogRef } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent  {

  email: string="";
  password: string="";
  loading:boolean = false;
  constructor(private router:Router, private toast: ToastrService, private authService: AuthService, public dialogRef: MatDialogRef<LoginComponent>) { }

  signOut() {
      this.authService.logout();
      this.toast.success("Successfully Logged Out")
  }

  signIn() {
    this.loading=true;
      var user = {
          email: this.email,
          password: this.password
      };


      this.authService.login(user).subscribe((sucesss: any) => {
          if (sucesss)
          {
              if (this.dialogRef && sucesss.token)
                  this.dialogRef.close(sucesss);
              else
                  this.toast.success(`${this.email} was not Logged In`);
          }
          this.dialogRef.close();
          this.loading= false
  });
  }

  cancel(){
    this.dialogRef.close();
  }

  forgotPassword(){
    this.cancel();
    console.log("forgotten");
  }

  saveUrl(){
    this.authService.setReturnUrl(this.router.url);
    this.cancel();
  }

}
