import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  searchTerm:string;
  constructor(private router:Router){}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(){
    localStorage.removeItem('token');
    console.log("Log Out");
  }

  search()
  {
    this.router.navigate(['Search',this.searchTerm]);
  }
}
