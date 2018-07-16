import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { RegisterViewModel } from '../ViewModels/registerViewModel';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  id: number;
  private sub: any;
  user: RegisterViewModel;
  videos: VideoViewModel[]= [];


  constructor(private route: ActivatedRoute, private http: HttpClient,private router:Router,private baseAddress:BaseAddressService){}
  ngOnInit() {
       this.id = this.route.snapshot.params['id'];  
      this.http.get<IServiceTypedResponse<RegisterViewModel>>(this.baseAddress.get()+"/userDetail/GetUser",{params:this.route.snapshot.params}).subscribe(x=>{
        if(x.status == 'good'){
               this.user = x.message;
               this.getUsersVideo();
        }
        if(x.status == 'bad'){

        }
      })
  }

  ngOnDestroy() {
  }

  getUsersVideo(){
    this.http.get<IServiceTypedResponse<VideoViewModel[]>>(this.baseAddress.get()+"/userDetail/GetUserVideo",{params: this.route.snapshot.params}).subscribe(x=>{
      if(x.status == 'good'){
             this.videos = x.message;
      }
      if(x.status == 'bad'){
      }
  });
}



  goToVideo(vid) {
    this.router.navigate(['video', vid.videoId]);
 };

}
