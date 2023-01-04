import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
//import * as internal from 'stream';
import { QuestionApiService } from '../api-services/question-api.service';
import { QuestionsResponse } from '../api-services/question-api.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  //questionsList!:Observable<QuestionsResponse>;
  //questions: any;
  questionList$!:Observable<any[]>;
  //questionResponse$!:Observable<any[[],number,number]>
  //onepagequestions$!:Observable<[any]>;
  constructor(private service:QuestionApiService) { }

  ngOnInit(): void {
    this.questionList$ = this.service.getQuestionList();
    //this.onepagequestions$ = this.questionsList$.questions;
  }

}
