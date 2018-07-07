import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent {
  constructor(private toastr: ToastrService, private router: Router,private route: ActivatedRoute,private http:HttpClient,private baseAddress:BaseAddressService) { }

  goToVideo(vid) {
     this.router.navigate(['video', vid.videoId]);
  };

  private sub: any;
  private whatType:string;



  isFullListDisplayed: Boolean = false;

  videos: VideoViewModel[]= [];
  onScroll() {
    console.log("fetching next 10");
    this.fetchVideos(10);
  }

  ngOnInit() {

    this.http.get(this.baseAddress.get() +"/Main/GetVideoList").subscribe(x=>console.log(x));
    console.log("paramters are")
    console.log(this.route.snapshot.params)
    this.whatType =  this.route.snapshot.params[0];
    this.fetchVideos();
    // this.sub = this.route.params.subscribe(params => {
    //   this.route.url.subscribe(x=>this.whatType = x && x[0] && x[0].path);
    //   this.fetchVideos();
    // });
  }

  fetchVideos(take?:Number){

    let query = `?type=${this.whatType}&skip=${this.videos.length.toString()}`;
   
   if(take){
     query+=`&take=${take.toString()}`;
   }
   

    console.log(this.baseAddress.get());
    this.http.get<IServiceTypedResponse<VideoViewModel[]>>(this.baseAddress.get() + `/Main/GetVideoList${query}`).subscribe(x=>
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
  }

}
