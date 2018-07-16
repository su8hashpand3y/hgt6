import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { BaseAddressService } from '../base-address.service';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  email:string;
  code:string;
  password:string;
  @ViewChild('stepper') stepper : any;
  constructor(private _formBuilder: FormBuilder,private http:HttpClient,private baseAddress:BaseAddressService,private toster:ToastrService) { }

  ngOnInit() {
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required]
    });
  }


   sendMail(){
     var data = {
      email: this.email,
      };  
          this.http.post<IServiceResponse>(this.baseAddress.get()+"/api/Login/PasswordResetEmail", data).subscribe(x => {
          if(x.status == "good"){
          this.stepper.next();
          }
          else{
             this.toster.error(x.message);
          }
          });
   }

   changePassword(){
    var data = {
      email: this.email,
      code:this.code,
      password:this.password
      };  
          this.http.post<IServiceResponse>(this.baseAddress.get()+"/api/Login/PasswordReset", data).subscribe(x => {
          if(x.status == "good"){
          this.stepper.next();
          }
          else{
             this.toster.error(x.message);
          }
          });
   }

}
