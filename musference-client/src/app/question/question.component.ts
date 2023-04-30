import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionApiService } from '../api-services/question-api.service';
import { Router, ActivatedRoute}  from '@angular/router';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  pageid:any ;
  form:FormGroup;
  params:any;
  searchtext:any;
  functionParam:any;
  searchParam:any;
  questions: any;
  questionList$!:Observable<any[]>;
  constructor(private service:QuestionApiService,private router:Router, private fb:FormBuilder,
    private activatedRoute:ActivatedRoute) {
      this.form=this.fb.group({
        search: ['',Validators.required]
      })
      this.activatedRoute.queryParams.subscribe(data=>{
        this.params = data;
        this.functionParam=this.params.function;
        this.searchParam=this.params.search;
      })
     }
  ifNewest(){
    if(this.questions.result.pages==1)return false;
     if(typeof this.functionParam === 'undefined')return true;
     else return false;
  }

  pagValue(){
    if(this.questions.result.currentPage==1){
      return 0;
    }
    if(this.questions.result.currentPage>1&&this.questions.result.currentPage<this.questions.result.pages){
      return 1;
    }
    else {
      return 2;
    }
  }
  next(){
    this.router.navigateByUrl(`/questions/${this.questions.result.currentPage+1}`);
  }
  previous(){
    this.router.navigateByUrl(`/questions/${this.questions.result.currentPage-1}`);
  }
  
  onSearch(){
    const val = this.form.value; 
    this.searchtext = val.search;
    this.router.navigate(['/questions'], {queryParams: {function: 'search',search: this.searchtext}});
  }
  mostLiked(){
    this.router.navigate(['/questions'], {queryParams: {function: 'mostliked'}});
  }
  bestUsers(){
    this.router.navigate(['/questions'], {queryParams: {function: 'bestusers'}});
  }
  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.pageid = this.activatedRoute.snapshot.paramMap.get('page')
    if(this.functionParam=='search'){
     this.service.searchQuestion(1,this.searchParam).subscribe(data=>{
      this.questions=data
    });
    }
    if(this.functionParam=='mostliked'){
      this.service.getAllQuestionsMostLiked(1).subscribe(data=>{
        this.questions=data
      });
    }
    else{
    this.service.getAllQuestionNewest(this.pageid).subscribe(data=>{
      this.questions=data
    });
  }
  }
  routetoask() {
    this.router.navigateByUrl('/askquestion');
    }
  }
