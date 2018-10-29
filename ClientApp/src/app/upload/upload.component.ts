import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

  constructor(private authService:AuthService) { }
  isTokenPresent:boolean= false;
  isAuthenticated: boolean = false;
  ngOnInit() {
    this.loginChecker();
  }

  loginChecker() {
    this.isTokenPresent = this.authService.isTokenPresent() ? true : false;
    if (this.isTokenPresent) {
      this.authService.isAuthorised().subscribe((x: boolean) => this.isAuthenticated = x);
    }
  }

  Login(){
    this.authService.openLoginDialog().subscribe(x=>{
      this.loginChecker();
    });
  }

}
