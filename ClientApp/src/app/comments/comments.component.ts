import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { CommentViewModel } from '../ViewModels/commentViewMode';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  @Input() videoId:string;
  UserComment:string;
  liked:boolean =false;

  comments:CommentViewModel[]=[
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
    { commentText : " This is a sample comment ,this has been written long deliberitly to see what happens when some comment is spannning multiple lines Thank you",userFirtName : "user test 1"},
  ];

  isAuthenticated:boolean;
  constructor(private http:HttpClient,private authService:AuthService) { }

  ngOnChanges(changes: SimpleChanges){
    let params = new HttpParams();
    params.append('id',this.videoId);
    this.http.get<IServiceTypedResponse<CommentViewModel[]>>('/video/GetComments',{params}).subscribe(x=>{
      if(x.status == 'good'){
        //this.comments = x.message;
      }
    });
  }

  comment(){
    this.http.post<IServiceResponse>('/video/Comment',{videoId:this.videoId, commentText: this.UserComment}).subscribe(x=>{
      if(x.status=='good'){
        this.comments.unshift({ commentText:this.UserComment,userFirtName:"Me"});
      }
    });
  }

  like(){
    this.liked = !this.liked;
    this.http.post<IServiceResponse>("/video/Like",{videoId:this.videoId}).subscribe(x=>{
      if(x.status == 'good'){
      }
      if(x.status == 'bad'){
      }
    });
  }

  ngOnInit() {
     this.isAuthenticated = this.authService.isAuthenticated() ?  true: false;
     console.log(this.isAuthenticated);
  }

  login(){
   this.authService.openLoginDialog().subscribe((x:any)=> this.isAuthenticated = this.authService.isAuthenticated() ?  true: false);

  }

}
