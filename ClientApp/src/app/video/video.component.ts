import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Location } from '@angular/common';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {

  id: number;
  private sub: any;
  video: VideoViewModel;
  loading: boolean;
  reported:boolean= false;
  constructor(private route: ActivatedRoute, private http: HttpClient, private location: Location,private router:Router,private baseAddress:BaseAddressService) { }

  ngOnInit() {
    this.loading= true;
    this.id = this.route.snapshot.params['id'];
    this.http.get<IServiceTypedResponse<VideoViewModel>>(this.baseAddress.get()+"/video/GetVideo",{params:this.route.snapshot.params}).subscribe(x=>{
      this.loading= false;
      if(x.status == 'good'){
             this.video = x.message;
      }
      if(x.status == 'bad'){

      }
    }, err => this.loading = false);
  }

  report(id){
    this.reported = true;
    let data = new FormData();
    data.append('videoId',id);
    this.http.post(this.baseAddress.get()+"/video/ReportVideo",data).subscribe(x=>console.log("Video reported successfuly"));
  }

  ngOnDestroy() {
  }

  gotoUser(userId){
    this.router.navigate(['user', userId]);
  }

  like(){
    this.sub = this.route.params.subscribe(params => {
    this.id = params['id'];
    this.http.get<IServiceResponse>(this.baseAddress.get()+"/video/Like",{params}).subscribe(x=>{
      if(x.status == 'good'){
      }
      if(x.status == 'bad'){

      }
      this.loading= false;
    }, err => this.loading = false)
  });
  }

  load() {
    this.loading = !this.loading;
  }
}
