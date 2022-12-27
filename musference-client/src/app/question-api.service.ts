 import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuestionApiService {

  readonly questionAPIUrl = "https://localhost:7289/api"
  constructor(private http:HttpClient) { }

  getQuestionList():Observable<any[]> {  
    return this.http.get<any>(this.questionAPIUrl+'/Question');
  }
  addQuestion(data:any) {
    return this.http.post<any>(this.questionAPIUrl+'/Question',data);
  }
  //wszystko w dół stad do naprawy
  getOneQuestion():Observable<any[]> {  
    return this.http.get<any>(this.questionAPIUrl+'/Question');
  }
  plusQuestion(data:any) {
    return this.http.post<any>(this.questionAPIUrl+'/Question',data);
  }
  minusQuestion():Observable<any[]> {  
    return this.http.get<any>(this.questionAPIUrl+'/Question');
  }
  addAnswer(data:any) {
    return this.http.post<any>(this.questionAPIUrl+'/Question',data);
  }
  plusAnswer():Observable<any[]> {  
    return this.http.get<any>(this.questionAPIUrl+'/Question');
  }
  minusAnswer(data:any) {
    return this.http.post<any>(this.questionAPIUrl+'/Question',data);
  }
}
 