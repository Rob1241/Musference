import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionApiService } from '../question-api.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  questionList$!:Observable<any[]>;

  constructor(private service:QuestionApiService) { }

  ngOnInit(): void {
    this.
    questionList$ = this.service.getQuestionList();
  }

}
