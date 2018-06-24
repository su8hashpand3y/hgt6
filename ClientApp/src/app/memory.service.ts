import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MemoryService {

  constructor() { }
  returnUrl:string;

  getReturnUrl(){
      let url = this.returnUrl;
      this.returnUrl = '';
      return url;
  }

  setReturnUrl(value:string){
      this.returnUrl = value;
  }
}
