import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(private http:HttpClient) { 

  }
  uploadImage(vals:any):Observable<any>{
    let data = vals;
    const headers = new HttpHeaders().set("InterceptorSkipHeader", '');
    return this.http.post('https://api.cloudinary.com/v1_1/da1tlcmhr/image/upload',data,{headers}
    );
  }
  uploadSound(vals:any):Observable<any>{
    let data = vals;
    const headers = new HttpHeaders().set("InterceptorSkipHeader", '');
    return this.http.post('https://api.cloudinary.com/v1_1/da1tlcmhr/auto/upload',data,{headers}
    );
  }
}
