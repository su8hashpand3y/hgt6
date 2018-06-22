import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent {
  constructor(private toastr: ToastrService, private router: Router,private route: ActivatedRoute) { }
   showSuccess(vid) {
     this.toastr.success('Hello world!', 'Toastr fun!');
     this.router.navigate(['video', vid]);
  };

  private sub: any;
  private whatType:String;



  isFullListDisplayed: Boolean = false;

  videos: string[] =
    ["https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_201866161557.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186616435.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186616452.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186783952.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186784010.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186784032.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_20186791823.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
      "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_2018679189.mp4",
    ];

  onScroll() {
    console.log("Scrolling...")
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.route.url.subscribe(x=>this.whatType = x && x[0] && x[0].path);
      //this.id = +params['id']; // (+) converts string 'id' to a number
      //this.http.get<>('videodetails').subscribe(x=>x.)
      // In a real app: dispatch action to load the details here.
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
