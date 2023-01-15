import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
//import * as internal from 'stream';
import { QuestionApiService } from '../api-services/question-api.service';
import { QuestionsResponse } from '../api-services/question-api.service';
import { Router, ActivatedRoute}  from '@angular/router';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { searchQuestion } from '../models/searchQuestion';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  //questionsList!:Observable<QuestionsResponse>;
  pageid:any ;
  form:FormGroup;
  params:any;
  searchtext:any;
  functionParam:any;
  searchParam:any;
  questions: any;
  questionList$!:Observable<any[]>;
  //questionResponse$!:Observable<any[[],number,number]>
  //onepagequestions$!:Observable<[any]>;
  constructor(private service:QuestionApiService,private router:Router, private fb:FormBuilder,
    private activatedRoute:ActivatedRoute) {
      this.form=this.fb.group({
        search: ['',Validators.required]
      })
      this.activatedRoute.queryParams.subscribe(data=>{
        this.params = data;
        this.functionParam=this.params.function;
        this.searchParam=this.params.search;
        //this.searchParam=this.
        //console.log(this.params.function)
        //console.log(data);
        //const val = this.form.value;
        //this.searchtext = val.search;
      })
     }
     
  // searchQuestion(){
  //   const val = this.form.value;
  //   this.searchtext = val.search;
  //   this.router.navigateByUrl('/questions?function=search');
  //   console.log(this.searchtext);
  //   this.service.searchQuestion(1,this.searchtext).subscribe(data=>{
  //     this.questions=data
  //   })

  //}
  
  onSearch(){
    const val = this.form.value; 
    this.searchtext = val.search;
    console.log(val.search);
    console.log("onsearchworks")
    this.router.navigate(['/questions'], {queryParams: {function: 'search',search: this.searchtext}});
  }
  ngOnInit(): void {
    
    this.pageid = this.activatedRoute.snapshot.paramMap.get('page')
    //const val = this.form.value;
    //let model =<searchQuestion>{};
    //this.searchtext = val.search;
    //console.log("valsearch = "+val);
    //console.log('pageid = '+ this.pageid);
    //console.log("function param = "+this.activatedRoute.snapshot.paramMap.get('function'));
    if(this.functionParam=='search'){
      //console.log("searchworks");
      //console.log("valsearch = "+val.search);
      //this.searchtext='string';
      console.log("searchtext = "+this.searchtext);
     this.service.searchQuestion(1,this.searchParam).subscribe(data=>{
      this.questions=data
      console.log(data);
    });
    }
    if(this.functionParam=='mostliked'){
      this.service.getAllQuestionsMostLiked(1).subscribe(data=>{
        this.questions=data
        console.log("moatliked");
      });
    }
    if(this.functionParam=='bestusers'){
      this.service.getAllQuestionsBestUsers(this.pageid).subscribe(data=>{
        this.questions=data
        console.log(data);
      });
    }
    else{
    console.log("SOMETHING");
    this.service.getAllQuestionNewest(this.pageid).subscribe(data=>{
      this.questions=data
      console.log(data);
    });
  }
  }
    //this.onepagequestions$ = this.questionsList$.questions;
  
  routetoask() {
    this.router.navigateByUrl('/askquestion');
    }
  }
