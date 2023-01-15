import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/login-model';
import jwt_decode from 'jwt-decode';
import {shareReplay, tap } from 'rxjs/operators';
import * as dayjs from 'dayjs'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  readonly trackAPIUrl ="https://localhost:7289/api";

  constructor(private http:HttpClient) { }
  Login(model: LoginModel) {  
    return this.http.post<any>(this.trackAPIUrl+'/User/Login',model)
    .pipe(tap((response:any)=>this.setSession(response))
    ,shareReplay());
  }

  private setSession(authResult: any) {
    const tokenInfo = this.getDecodedAccessToken(authResult.result);
    //const tokenInfo = this.parseJwt(authResult.result);
    const expiresAt = tokenInfo.exp;
    //console.log(tokenInfo.exp);
    //console.log(tokenInfo);
    const decodedId = tokenInfo['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
    localStorage.setItem('id_token', authResult.result);
    localStorage.setItem('expires_at', expiresAt);
    localStorage.setItem('user_id', decodedId)
  }
  getDecodedAccessToken(token: string): any {
    try {
      return jwt_decode(token);
    } catch(Error) {
      return null;
    }
  }
  public isLoggedIn(){
    if(!this.isToken){return false}
    var date1 = new Date(Date.now());
    var date2 = new Date(this.getExpiration()*1000)
    return dayjs(date1).isBefore(date2);
  }
  isLoggedOut(){
    return !this.isLoggedIn();
  }

  isToken(){
    if(localStorage.getItem("id_token")){
      return true;
    }
    return false;
  }

  logout(){
    localStorage.removeItem("id_token");
    localStorage.removeItem("expires_at");
    localStorage.removeItem("user_id");
  }
  getExpiration(){
    return Number(localStorage.getItem("expires_at"))
  }
}
