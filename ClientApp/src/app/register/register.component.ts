import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { MatDialog } from '@angular/material';
import { AvatarPickerComponent } from '../avatar-picker/avatar-picker.component';
import { HttpClient } from '@angular/common/http';
import { RegisterViewModel } from '../ViewModels/registerViewModel';
import { ICapthaResponse } from '../ViewModels/ICapthaResponse';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { ToastrService } from 'ngx-toastr';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { MemoryService } from '../memory.service';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  loading:boolean;
  email:string;
  avatarImage:string;
  firstName:string;
  middleName:string;
  lastName:string;
  age:Number;
  password:string;
  gender:string;
  district:string;
  town:string;
  capthaId:Number;
  capthaText:string
  captha:string;
  
  confirmPassword:string;
  confirmEmail:string;


  errors:string;

  genders = [
    'Male',
    'Female',
    'Other',
  ];

  districts =['Bilaspur','Chamba','Hamirpur','kangra','Kinnaur','Kullu','Lahaul and Spiti','Mandi','Shimla','Sirmaur','Solan','Una'];
  constructor(private router: Router,private memory:MemoryService, private dialog: MatDialog,private http:HttpClient,private toaster:ToastrService,private baseAddress:BaseAddressService) {
  
   }

  ngOnInit() {
    this.http.get<IServiceTypedResponse<ICapthaResponse>>(this.baseAddress.get() +"/api/Login/getCaptha").subscribe( x => {
     
    console.log(x)
    if(x.status == 'ok'){
      this.capthaId = x.message.capthaId;
      this.capthaText = x.message.capthaText;
    }
    if(x.status == 'error'){
      this.errors = 'Please Referesh Page,Somthing gone wrong in captha Generation';
    }
  });
}

  openDialog(): void {
    let dialogRef = this.dialog.open(AvatarPickerComponent);

    dialogRef.afterClosed().subscribe(result => {
        this.avatarImage = result;
    });
 }


  register(){
   this.errors = "";
    if(this.email !== this.confirmEmail){
          this.errors = "Email Dont Match";
          return;
    }

    if(this.password !== this.confirmPassword){
      this.errors = "Password Dont Match";
      return;
}

    console.log("Registering");
    
    this.loading=true;
      let rvm : RegisterViewModel= {
      email : this.email,
      avatarImage : this.avatarImage,
      firstName : this.firstName,
      middleName : this.middleName,
      lastName : this.lastName,
      age : this.age,
      password : this.password,
      gender : this.gender,
      district : this.district,
      town : this.town,
      capthaId : this.capthaId,
      captha : this.captha,
    }
    
    this.http.post<IServiceResponse>(this.baseAddress.get() +"/api/Login/Register",rvm).subscribe(x=> {
      if(x.status == 'registerd'){
        localStorage.setItem('token', x.message);
        this.toaster.info(`Thank You.You are logged in as ${this.firstName}`);
        this.router.navigateByUrl(this.memory.getReturnUrl() || '');
      }
      if(x.status == 'error'){
        this.errors = x.message;
      }
      this.loading = false;
    });
    
  }

  cancel(){
    this.router.navigateByUrl('/Home');
  }
}
