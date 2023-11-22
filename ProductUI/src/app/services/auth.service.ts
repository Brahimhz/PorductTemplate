import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  private baseUrl:string = "https://localhost:7111/api/Authentication/" ;

  signUp (userObj: any){
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post <any>(`${this.baseUrl}register`, userObj, { headers });
  }

  login (loginObj: any){
    return this.http.post<any>(`${this.baseUrl}login`, loginObj);
  }

}
