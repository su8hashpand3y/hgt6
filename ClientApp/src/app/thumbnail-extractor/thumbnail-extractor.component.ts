import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import Iloader from '../Iloader';
import { HttpClient } from '@angular/common/http';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BaseAddressService } from '../base-address.service';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { ICapthaResponse } from '../ViewModels/ICapthaResponse';


@Component({
  selector: 'app-thumbnail-extractor',
  templateUrl: './thumbnail-extractor.component.html',
  styleUrls: ['./thumbnail-extractor.component.css']
})
export class ThumbnailExtractorComponent implements OnInit,Iloader {

  loading:boolean = false;
  w:any;
  h:any;
  context:any;
  video:any;
  canvas:any;
  @ViewChild('video') videoElement:ElementRef;
  @ViewChild('canvas') canvasElement:ElementRef;


  capthaId:Number;
  capthaText:string
  captha:string;

  videoUrl:string;
  name:string;
  description:string;
  category:string;
  posterUrl:string;
  categories:string[] = ['Film & Animation','Autos & Vehicles','Music','Pets & Animals','Sports','Travel & Events','Gaming','People & Blogs','Comedy','Entertainment','News & Politics','How-to & Style','Education','Science & Technology','Non-profits & Activism','OTHER'];
 
  max:Number;
  loadedToStore:boolean= false;
  errors:string;

  constructor(private http:HttpClient,private router:Router,private toast:ToastrService,private baseAddress:BaseAddressService) { }

  ngOnInit() {
    this.video = this.videoElement.nativeElement as HTMLVideoElement;
    this.canvas = this.canvasElement.nativeElement as HTMLCanvasElement;
    this.context = this.canvas.getContext('2d');
    this.http.get<IServiceTypedResponse<ICapthaResponse>>(this.baseAddress.get() +"/api/Login/getCaptha").subscribe( x => {
     
      console.log(x)
      if(x.status == 'ok'){
        this.capthaId = x.message.capthaId;
        this.capthaText = x.message.capthaText;
      }
      if(x.status == 'error'){
        this.errors = 'Please Referesh Page,Somthing gone wrong in captha Generation';
      }
    });
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
    this.errors ="";
    var file = event.target.files[0];
    console.log(file);
    if(file){
      if(file.size > 1048576 * 500){
        //greater than 500 MB
        this.errors = "We dont support Big(> 500 MB) File"
        console.log("File is too Big");
        return;
      }
      // upload the video and show a loader till it is uploaded
       this.loading = true;
       let formData: FormData = new FormData();
       formData.append('file', file);
       console.log("uploading to server")
      let ext = (/[.]/.exec(file.name)) ? /[^.]+$/.exec(file.name) : undefined;
       console.log(ext);
      if(!ext){
        this.errors ="Cant Read Extension";
        return;
      }

    

      let headers: any = new Headers();
      headers.set('Content-Type', 'multipart/form-data');


       this.http.post<IServiceResponse>(this.baseAddress.get()+"/api/Upload/UploadFileToStore?ext="+ext,formData).subscribe(x=>{
         console.log(x);
         if(x.status =='success'){
           this.videoUrl=x.message; 
           this.loadedToStore = true;
         }
         if(x.status =='error'){
           this.errors = x.message;
        }

        this.loading=false;
       },(e)=>{
        console.log(e);
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
      this.errors = "";
      if(!this.capthaText || this.capthaText == ""){
              this.errors = "Captha is Mandatory!";
              return;
      }

      if(!this.name || this.name == ""){
        this.errors = "Name is Mandatory!";
        return;

}

if(!this.description || this.description == ""){
  this.errors = "Description is Mandatory!";
  return;

}


if(!this.category || this.category == ""){
  this.errors = "Category is Mandatory!";
  return;

}

              this.http.post<IServiceResponse>(this.baseAddress.get()+"/api/Upload/upload", {
                title:this.name,
                description:this.description,
                category:this.category,
                videoUrl:this.videoUrl,
                posterUrl:this.posterUrl,
                capthaId : this.capthaId,
                captha : this.captha,
              })
                  .subscribe(
                  x => {
                    console.log(x);
                      if(x.status == 'success'){
                      this.toast.success(`Your video ${this.name} was uploaded successfully.`);
                      this.router.navigateByUrl(`/video/${x.message}`);
                   }
                   
                   if(x.status == 'error'){
                     this.errors = x.message;
                   }});
      }
}
