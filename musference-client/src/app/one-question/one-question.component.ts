import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionApiService } from '../api-services/question-api.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AnswerQuestionModel } from '../models/answer-question-model';

@Component({
  selector: 'app-one-question',
  templateUrl: './one-question.component.html',
  styleUrls: ['./one-question.component.css']
})
export class OneQuestionComponent implements OnInit {


  question:any;
  id:any;
  page:any;
  form:FormGroup
  constructor(private service:QuestionApiService,private activatedRoute:ActivatedRoute,private fb:FormBuilder) 
  { 
    this.form=this.fb.group({
      content: ['',Validators.required]});
  }
  isOwner(id:Number){
    if(Number(localStorage.getItem('user_id'))==id)return true
    else return false;
  }
  addAnswer(){
    const val = this.form.value;
    let model =<AnswerQuestionModel>{};
    model.content=val.content;
    let quest_id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.service.addAnswer(model,quest_id).subscribe();
  }
  likeQuestion(question_id:Number){
    this.service.plusQuestion(question_id).subscribe();
  }
  likeAnswer(answer_id:Number){
    this.service.plusAnswer(answer_id).subscribe();
  }
  deleteQuestion(question_id:Number){
    this.service.deleteQuestion(question_id).subscribe();
  }
  deleteAnswer(answer_id:Number){
    this.service.deleteAnswer(answer_id).subscribe();
  }
  ngOnInit(): void {
    this.id =this.activatedRoute.snapshot.paramMap.get('id');
    this.page = this.activatedRoute.snapshot.paramMap.get('page')
    this.service.getQuestion(this.id,this.page).subscribe(data=>{
      console.log(data)
      this.question=data
    });
    
  }

}
