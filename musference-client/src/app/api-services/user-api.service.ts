import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  readonly trackAPIUrl = "https://localhost:7289/api"
  private _isLoggedIn$ = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this._isLoggedIn$.asObservable();
  constructor(private http:HttpClient) {
    const token = localStorage.getItem('jwt');
    this._isLoggedIn$.next(!!token);
   }

  
  getAllUsersNewest(page: number):Observable<any> {
    return this.http.get<any>(this.trackAPIUrl+`/User/newest/${page}`);
  }
  getAllUsersReputation(page: number):Observable<any> {
    return this.http.get<any>(this.trackAPIUrl+`/User/reputation/${page}`);
  }
  searchUsers(page:Number,text:Text):Observable<any>{
    return this.http.get<any>(this.trackAPIUrl+`/User/search/${page}/${text}`);
  }
  getUser(userId:number):Observable<any>{  
    return this.http.get<any>(this.trackAPIUrl+`/User/${userId}`);
  }
  Signup(data:any) {
    return this.http.post<any>(this.trackAPIUrl+'/User/Signup',data);
  }
  deleteUser(data:any){
    return this.http.delete(this.trackAPIUrl+'/User',{ body: data});
  }
  changePassword(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangePassword', data);
  }
  changeName(data:any) {
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeName',data);
  }
  changeDescription(data:any) {
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeDescription',data);
  }
  changeEmail(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeEmail',data);
  }
  changeCity(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeCity',data);
  }
  changeCountry(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeCountry',data);
  }
  changeContact(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeContact',data);
  }
  changeImage(data:any) {  
    return this.http.put<any>(this.trackAPIUrl+'/User/ChangeImage',data);
  }
}