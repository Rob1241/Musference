import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionApiService } from '../api-services/question-api.service';
import { AddQuestionModel } from '../models/question-model';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.css']
})
export class AskQuestionComponent implements OnInit {
  form:FormGroup

  constructor(private fb:FormBuilder,private service:QuestionApiService) { 
    this.form= this.fb.group({
      heading: ['',Validators.required],
      content: ['',Validators.required]
  });
  }
  addQuestion(){
  const val =this.form.value;
    let model = <AddQuestionModel>{};
    model.heading = val.heading;
    model.content = val.content;
    this.service.addQuestion(model).subscribe();
  }
  ngOnInit(): void {

    
  }

}
