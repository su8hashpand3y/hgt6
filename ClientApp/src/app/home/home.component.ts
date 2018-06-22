import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent {
  constructor(private toastr: ToastrService, private router: Router,private route: ActivatedRoute,private http:HttpClient) { }

  goToVideo(vid) {
     this.router.navigate(['video', vid.videoId]);
  };

  private sub: any;
  private whatType:string;



  isFullListDisplayed: Boolean = false;

  videos: VideoViewModel[]= [];
  onScroll() {
    this.fetchVideos(10);
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.route.url.subscribe(x=>this.whatType = x && x[0] && x[0].path);
      this.fetchVideos();
    });
  }

  fetchVideos(take?:Number){
    let params = new HttpParams();
    params.set("type",this.whatType);
    params.set("skip",this.videos.length.toString());
    if(take){
    params.set("take",take.toString());
    }

    this.http.get<IServiceTypedResponse<VideoViewModel[]>>(`/Main/GetVideoList`,{ params}).subscribe(x=>
      {
        if(x.status == "good")
        {
        this.videos.push(...x.message)
        if(x.message.length < 10){
          this.isFullListDisplayed = true;
        }
        }
        if(x.status == "bad")
        {
             this.toastr.error(`Problem Loading Videos`);
        }
      });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
