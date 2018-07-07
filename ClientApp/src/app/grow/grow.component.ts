import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { BaseAddressService } from '../base-address.service';

@Component({
  selector: 'app-grow',
  templateUrl: './grow.component.html',
  styleUrls: ['./grow.component.css']
})
export class GrowComponent implements OnInit {

  feedback:string;
  constructor(private toster:ToastrService,private http: HttpClient,private baseRef:BaseAddressService) { }

  ngOnInit() {
  }

  giveFeedback(){
    let data = new FormData();
    data.append('message',this.feedback);
    this.http.post(this.baseRef.get()+'/Main/Feedback',data).subscribe(x=>console.log(x));
    this.feedback ="";
    this.toster.info("Thank you for your valuable feedback");
  }

}
