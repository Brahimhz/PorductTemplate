import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl: string = 'https://localhost:7146/api/Product/';
  constructor(private http: HttpClient,private auth:AuthService){}
  getProducts() {

    const myToken = this.auth.getToken();

    const requestOptions = {
      headers: {
        Authorization: 'Bearer ' + myToken,
      },
    };

    return this.http.get<any>(this.baseUrl+ 'Products',requestOptions);
  }

}
