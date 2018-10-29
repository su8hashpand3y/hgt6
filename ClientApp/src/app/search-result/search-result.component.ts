import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseAddressService } from '../base-address.service';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { VideoViewModel } from '../ViewModels/videoViewModel';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent implements OnInit {

  id: number;
  loading: boolean;
  videos: VideoViewModel[]= [];
  isFullListDisplayed: Boolean = false;


  constructor(private toastr: ToastrService,private route: ActivatedRoute, private http: HttpClient,private router:Router,private baseAddress:BaseAddressService) { }

  goToVideo(vid) {
    this.router.navigate(['video', vid.videoId]);
 };

 onScroll() {
  this.fetchVideos(10);
}

  ngOnInit() {
    //this.loading= true;
   

    this.route.params.subscribe(x => {
      this.fetchVideos();
    });
    //this.http.get<IServiceTypedResponse<VideoViewModel[]>>(this.baseAddress.get() + `/Main/SearchVideo?searchTerm=${this.route.snapshot.params['id']}`).subscribe(x=>
    //  {
    //  this.loading = false;
    //  if (x.status == "good")
    //    {
    //    this.videos.push(...x.message)
    //    if(x.message.length < 10){
    //      this.isFullListDisplayed = true;
    //    }
    //    }
    //    if(x.status == "bad")
    //    {
    //         this.toastr.error(`Problem Loading Videos`);
    //  }
    //},e => this.loading = false);

    //this.fetchVideos();
  }

  fetchVideos(take?: Number) {
    this.id = this.route.snapshot.params['id'];
    this.videos = [];
    this.http.get<IServiceTypedResponse<VideoViewModel[]>>(this.baseAddress.get() + `/Main/SearchVideo?searchTerm=${this.id}&skip=${this.videos.length}`).subscribe(x=>
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

}
