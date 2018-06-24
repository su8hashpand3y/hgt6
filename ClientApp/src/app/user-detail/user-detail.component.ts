import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { RegisterViewModel } from '../ViewModels/registerViewModel';

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


  constructor(private route: ActivatedRoute, private http: HttpClient,private router:Router){}
  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      console.log(params);
      this.http.get<IServiceTypedResponse<RegisterViewModel>>("/userDetail/GetUser",{params}).subscribe(x=>{
        if(x.status == 'good'){
               this.user = x.message;
        }
        if(x.status == 'bad'){

        }
      })
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  getUsersVideo(){
    let params = new HttpParams();
    params.append('id',this.id.toString());
    this.http.get<IServiceTypedResponse<VideoViewModel[]>>("/userDetail/GetUserVideo",{params}).subscribe(x=>{
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
