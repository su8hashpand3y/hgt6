import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  searchTerm:string;
  constructor(private router:Router,private toster:ToastrService){}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(){
    localStorage.removeItem('token');
    this.toster.info("Logout Succesful");
  }

  search()
  {
    this.router.navigate(['Search',this.searchTerm]);
  }
}
