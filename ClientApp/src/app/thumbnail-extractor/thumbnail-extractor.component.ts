import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import Iloader from '../Iloader';
import { HttpClient } from '@angular/common/http';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-thumbnail-extractor',
  templateUrl: './thumbnail-extractor.component.html',
  styleUrls: ['./thumbnail-extractor.component.css']
})
export class ThumbnailExtractorComponent implements OnInit,Iloader {

  loading:boolean = false;
  videoLocalPath: string = "https://s3.ap-south-1.amazonaws.com/hgtdata/e8b117af-de0c-4791-889d-75a3cd0b2616_201866161557.mp4";

  w:any;
  h:any;
  context:any;
  video:any;
  canvas:any;
  @ViewChild('video') videoElement:ElementRef;
  @ViewChild('canvas') canvasElement:ElementRef;


  videoUrl:string;
  name:string;
  description:string;
  category:string;
  posterUrl:string;
  categories:string[] = ['music','comedy','other'];
 
  max:Number;
  loadedToStore:boolean= false;
  errors:string;

  constructor(private http:HttpClient,private router:Router,private toast:ToastrService) { }

  ngOnInit() {
    this.video = this.videoElement.nativeElement as HTMLVideoElement;
    this.canvas = this.canvasElement.nativeElement as HTMLCanvasElement;
		this.context = this.canvas.getContext('2d');
  }

  cancel(){
    this.router.navigateByUrl('/Home');
  }

  onInputChange(event: any) {
    if( this.video){

    this.video.currentTime = event.value;
    setTimeout(x=>this.snap(),500);
    }
  }

  loadedmetadata(){
    let ratio = this.video.videoWidth / this.video.videoHeight;
    // Define the required width as 100 pixels smaller than the actual video's width
    this.w = this.video.videoWidth;
    // Calculate the height based on the video's width and the ratio
    let y = this.w / ratio;
    this.h = parseInt(y.toString(), 10);
    // Set the canvas width and height to the values just calculated
    this.canvas.width = 300;
    this.canvas.height = 200;	
    this.max = this.video.duration;
    console.log(this.video.duration);
    this.canvas.crossOrigin = "Anonymous";
    this.video.currentTime= this.video.duration / 2;
    setTimeout(x=>this.snap(),500);
  }
  
  videoSelected(event){
    var file = event.target.files[0];
    console.log(file);
    if(file){
      if(file.size > 1048576 * 24){
        //greater than 500 MB
        this.errors = "We dont support Big File"
        console.log("File is too Big");
      }
      // upload the video and show a loader till it is uploaded
       this.loading = true;
       let formData: FormData = new FormData();
       formData.append('file', file, file.name);
       this.http.post<IServiceResponse>("/api/Upload/UploadFileToStore",formData).subscribe(x=>{
         if(x.status =='success'){
           this.videoUrl=x.message; 
           this.loadedToStore = true;
         }
         if(x.status =='error'){
           this.errors = x.message;
        }

        this.loading=false;
       });
      // console.log(this.videoLocalPath);
    }
    else{
    }
    }

		// Takes a snapshot of the video
		 snap() {
       console.log("snapping..");
			// Define the size of the rectangle that will be filled (basically the entire element)
			this.context.fillRect(0, 0, this.w, this.h);
      // Grab the image from the video
       this.context.drawImage(this.video, 0, 0, 300, 200);
           
       this.posterUrl = this.canvas.toDataURL();
    }
    

    upload() {
              this.http.post<IServiceResponse>("/api/Upload/upload", {
                name:this.name,
                description:this.description,
                category:this.category,
                videoUrl:this.videoUrl,
                posterUrl:this.posterUrl
              })
                  .subscribe(
                  x => {
                      if(x.status == 'success'){
                      this.toast.success(`Your video ${this.name} was uploaded successfully.`);
                      this.router.navigateByUrl(`/Video/${x.message}`);
                   }
                   if(x.status == 'error'){
                     this.errors = x.message;
                   }});
      }
}
