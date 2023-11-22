import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {  Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userPayload: any;
  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.userPayload = this.decodedToken();
  }

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

  signOut(){
    localStorage.clear();
    this.router.navigate(['login']);
  }

  storeToken (tokenValue: string)
  {
    localStorage.setItem('token', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token')
  }


  decodedToken() {
    const helper = new JwtHelperService();
    return helper.decodeToken(this.getToken()!);
  }

  getFullNameFromToken() {
    if (this.userPayload) return this.userPayload.unique_name;
  }

  getRoleFromToken() {
    if (this.userPayload) return this.userPayload.role;
  }


}
