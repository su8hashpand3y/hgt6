import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { IServiceResponse } from '../ViewModels/IServiceResponse';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {

  id: number;
  private sub: any;
  video: VideoViewModel ={numberOfLikes:1,numberOfViews:1, description:"",title:"",userDistrict:"",userFirstName:"",userId:"",userLastName:"",userTown:"", videoId:"",videoUrl:"https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186616435.mp4",posterUrl:""};
  loading: boolean;
  constructor(private route: ActivatedRoute, private http: HttpClient, private location: Location,private router:Router) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      console.log(params);
      this.http.get<IServiceTypedResponse<VideoViewModel>>("/video/GetVideo",{params}).subscribe(x=>{
        if(x.status == 'good'){
               this.video = x.message;
        }
        if(x.status == 'bad'){

        }
        this.loading= false;
      })
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  gotoUser(userId){
    this.router.navigate(['video', userId]);
  }

  like(){
    this.sub = this.route.params.subscribe(params => {
    this.id = params['id'];
    this.http.get<IServiceResponse>("/video/Like",{params}).subscribe(x=>{
      if(x.status == 'good'){
      }
      if(x.status == 'bad'){

      }
      this.loading= false;
    })
  });
  }

  load() {
    this.loading = !this.loading;
  }
}
