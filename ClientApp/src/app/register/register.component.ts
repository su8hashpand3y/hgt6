import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../auth.service';
import { MatDialog } from '@angular/material';
import { AvatarPickerComponent } from '../avatar-picker/avatar-picker.component';
import { HttpClient } from '@angular/common/http';
import { RegisterViewModel } from '../ViewModels/registerViewModel';
import { ICapthaResponse } from '../ViewModels/ICapthaResponse';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { ToastrService } from 'ngx-toastr';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';

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

  errors:string;

  genders = [
    'Male',
    'Female',
    'Other',
  ];

  districts =['Shimla','Solan'];
  constructor(private router: Router,private authService:AuthService, private dialog: MatDialog,private http:HttpClient,private toaster:ToastrService) {
  
   }

  ngOnInit() {
    this.http.get<IServiceTypedResponse<ICapthaResponse>>("http://localhost:54412/api/Login/getCaptha").subscribe( x => {
     
    console.log(x)
    if(x.status == 'ok'){
      this.capthaId = x.message.capthaId;
      this.capthaText = x.message.capthaText;
    }
    if(x.status == 'error'){
      this.errors = 'Please Referesh Page,Somthing gone wrong in caotha Generation';
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
    
    this.http.post<IServiceResponse>("http://localhost:54412/api/Login/Register",rvm).subscribe(x=> {
      if(x.status == 'registerd'){
        localStorage.setItem('token', x.message);
        this.toaster.info(`Thank You.You are logged in as ${this.firstName}`);
        this.router.navigateByUrl(this.authService.getReturnUrl() || '');
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
