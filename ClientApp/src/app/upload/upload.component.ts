import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

  constructor(private authService:AuthService) { }
  isAuthenticated:boolean= false;
  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated() ? true : false;
  }

  Login(){
    this.authService.openLoginDialog().subscribe(x=>{
    this.isAuthenticated = this.authService.isAuthenticated() ? true : false;
    });
  }

}
