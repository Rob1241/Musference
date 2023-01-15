 import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddQuestionModel } from '../models/question-model';

export interface QuestionsResponse {
  questions: Array<any>;
  pages: number;
  currentPage: number;
}

@Injectable({
  providedIn: 'root'
})
export class QuestionApiService {

  readonly questionAPIUrl = "https://localhost:7289/api"
  constructor(private http:HttpClient) { }
  
  getAllQuestionNewest(page: number):Observable<any> {  
    return this.http.get<any>(this.questionAPIUrl+`/Question/${page}`);
  }
  getAllQuestionsMostLiked(page:number):Observable<any> {  
    return this.http.get<any>(this.questionAPIUrl+`/Question/most-liked/${page}`);
  }
  getAllQuestionsBestUsers(page:number):Observable<any> {  
    return this.http.get<any>(this.questionAPIUrl+`/Question/best-users/${page}`);
  }
  searchQuestion(page:Number,text:Text):Observable<any>{
    return this.http.get<any>(this.questionAPIUrl+`/Question/Search/${page}/${text}`);
  }
  addQuestion(data:any) {
    return this.http.post<any>(this.questionAPIUrl+'/Question',data);
  }
  getQuestion(id:number, page:number):Observable<any> {  
    return this.http.get<any>(this.questionAPIUrl+`/Question/OneQuestion/${id}/${page}`);
  }
  plusQuestion(data:any) {
    return this.http.put<any>(this.questionAPIUrl+`/Question/Plus/${data}`,null);
  }
  addAnswer(data:any,id:number) {
    return this.http.post<any>(this.questionAPIUrl+`/Question/${id}/Answer`,data);
  }
  plusAnswer(data:Number) {  
    return this.http.put<any>(this.questionAPIUrl+`/Question/Answer/${data}/Plus`,null);
  }
  deleteQuestion(id:Number){
    return this.http.delete<any>(this.questionAPIUrl+`/Question/${id}/Delete`)
  }
  deleteAnswer(id:Number){
    return this.http.delete<any>(this.questionAPIUrl+`/Question/Answer/${id}/Delete`)
  }
}
 