import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseAddressService {

  get():string{
    if (!environment.production) {
    return "http://localhost:54412";
    }

    return "";
  }
}
