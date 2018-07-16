import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { IServiceTypedResponse } from '../ViewModels/IServiceTypedResponse';
import { CommentViewModel } from '../ViewModels/commentViewMode';
import { IServiceResponse } from '../ViewModels/IServiceResponse';
import { AuthService } from '../auth.service';
import { BaseAddressService } from '../base-address.service';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  @Input() videoId:string;
  @Input() isLikedByMe:boolean;
  UserComment:string;
  liked:boolean =false;
  loading:boolean=false;
  comments:CommentViewModel[]= [];

  isAuthenticated:boolean;
  constructor(private http:HttpClient,private authService:AuthService,private baseAddress:BaseAddressService) { }

  ngOnChanges(changes: SimpleChanges){
    this.http.get<IServiceTypedResponse<CommentViewModel[]>>(this.baseAddress.get()+ `/Video/GetComments?id=${this.videoId}`).subscribe(x=>{
      if(x.status == 'good'){
        this.comments = x.message;
      }
    });
  }

  deleteComment(comment: CommentViewModel) {
    this.loading= true;
    let data = new FormData();
    console.log(comment.id);
    data.append('commentId', comment.id);
    this.http.post<IServiceResponse>(this.baseAddress.get()+'/Video/DelComment',data).subscribe(x=>{
         this.loading= false;
        var index = this.comments.indexOf(comment);
            if (index > -1) {
              this.comments.splice(index, 1);
      }
    }, err => this.loading = false);
  }

  comment(){
    let data = new FormData();
    data.append('videoId',this.videoId);
    data.append('commentText',this.UserComment);
    this.loading = true;
    this.comments.unshift({ commentText: this.UserComment, userFirstName: "Me", id: null});
        this.UserComment = "";
    this.http.post<IServiceResponse>(this.baseAddress.get() + '/Video/Comment', data).subscribe(x => {
      if (x.status == 'good') {

      }
      this.loading = false;
    }, err => this.loading = false);
  }

  like(){
    let data = new FormData();
    data.append('videoId',this.videoId);
    this.liked = !this.liked;
    this.http.post<IServiceResponse>(this.baseAddress.get()+ "/Video/Like",data).subscribe(x=>{
      if(x.status == 'good'){
        // output liked
      }
      if(x.status == 'bad'){
      }
    });
  }

  ngOnInit() {
    this.liked = this.isLikedByMe;
     this.isAuthenticated = this.authService.isAuthenticated() ?  true: false;
  }

  login(){
   this.authService.openLoginDialog().subscribe((x:any)=> this.isAuthenticated = this.authService.isAuthenticated() ?  true: false);

  }

}
