import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {

  id: number;
  private sub: any;
  videoUrl: string;
  loading: boolean;
  constructor(private route: ActivatedRoute, private http: HttpClient, private location: Location) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.videoUrl = params['id'];
      console.log(this.location.path());
      //this.id = +params['id']; // (+) converts string 'id' to a number
      //this.http.get<>('videodetails').subscribe(x=>x.)
      // In a real app: dispatch action to load the details here.
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }


  load() {
    this.loading = !this.loading;
  }
}
