import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionApiService } from '../api-services/question-api.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AnswerQuestionModel } from '../models/answer-question-model';
import { AuthService } from '../api-services/auth.service';
import { UploadService } from '../services/upload.service';

@Component({
  selector: 'app-one-question',
  templateUrl: './one-question.component.html',
  styleUrls: ['./one-question.component.css']
})
export class OneQuestionComponent implements OnInit {


  question:any;
  id:any;
  page:any;
  form:FormGroup;
  imageUrl:any;
  audioUrl:any;
  audio:any;
  time:any = 0;
  constructor(private upload: UploadService,private service:QuestionApiService,private activatedRoute:ActivatedRoute, private authService:AuthService,private router:Router,private fb:FormBuilder) 
  { 
    this.form=this.fb.group({
      content: ['',Validators.required]});
  }
  pagValue(){
    if(this.question.result.currentPage==1){
      return 0;
    }
    if(this.question.result.currentPage>1&&this.question.result.currentPage<this.question.result.pages){
      return 1;
    }
    else {
      return 2;
    }
  }
  ifOnePage(){
    if(this.question.result.pages==1)return false;
    else return true;
  }
  ifAnswers(){
    if(this.question.result.answersAmount==0)return false;
    else return true;
  }
  next(){
    this.router.navigateByUrl(`/onequestion/${this.question.result.question.id}/${this.question.result.currentPage+1}`);
  }
  previous(){
    this.router.navigateByUrl(`/onequestion/${this.question.result.question.id}/${this.question.result.currentPage-1}`);
  }
  isOwner(id:Number){
    if(Number(localStorage.getItem('user_id'))==id&&this.authService.isLoggedIn())return true
    else return false;
  }

  files: File[] = [];

	onSelect(event:any) {
		this.files.push(...event.addedFiles);
	}

	onRemove(event:any) {
		this.files.splice(this.files.indexOf(event), 1);
	}

  filesAudio: File[] = [];

	onSelectAudio(event:any) {
		this.filesAudio.push(...event.addedFiles);
	}

	onRemoveAudio(event:any) {
		this.filesAudio.splice(this.filesAudio.indexOf(event), 1);
	}

  uploadImage(){
    if(!this.files[0]){
      alert('Image not found')
    }
    const file_data = this.files[0];
    const data = new FormData();
    data.append('file',file_data);
    data.append('upload_preset','musference');
    data.append('cloud_name','da1tlcmhr');
    this.upload.uploadImage(data).subscribe((response)=>{
    if(response){
      this.imageUrl = response.secure_url
    }
    });
  }
  
  uploadAudio(){
    if(!this.filesAudio[0]){
      alert('Sound not found')
    }
    const file_data = this.filesAudio[0];
    const data = new FormData();
    data.append('file',file_data);
    data.append('upload_preset','musference');
    data.append('cloud_name','da1tlcmhr');
    this.upload.uploadSound(data).subscribe((response)=>{
    if(response){
      this.audioUrl = response.secure_url
    }
    });
  }
  playSound(source:any){
    this.audio = new Audio();
    this.audio.src = source;
    this.audio.load();
    this.audio.currentTime = this.time;
    this.audio.play(this.time);
  }
  pauseSound(){
    this.time = this.audio.currentTime;
    this.audio.pause();
  }
  stopSound(){
    this.time = 0;
    this.audio.currentTime = 0;
    this.audio.pause();
  }
  addAnswer(){
    const val = this.form.value;
    let model =<AnswerQuestionModel>{};
    model.content=val.content;
    model.audioFile = this.audioUrl;
    model.imageFile = this.imageUrl;
    let quest_id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.service.addAnswer(model,quest_id).subscribe();
    this.audioUrl = null;
    this.imageUrl = null;
    window.location.reload();
  }


  likeQuestion(question_id:Number){
    if(this.authService.isLoggedIn()){
    this.service.plusQuestion(question_id).subscribe();
    window.location.reload();
  }
  }
  likeAnswer(answer_id:Number){
    if(this.authService.isLoggedIn()){
    this.service.plusAnswer(answer_id).subscribe();
    window.location.reload();
  }
  }
  deleteQuestion(question_id:Number){
    this.service.deleteQuestion(question_id).subscribe();
    this.router.navigateByUrl('/questions/1');
  }
  deleteAnswer(answer_id:Number){
    this.service.deleteAnswer(answer_id).subscribe();
    window.location.reload();
  }
  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.id =this.activatedRoute.snapshot.paramMap.get('id');
    this.page = this.activatedRoute.snapshot.paramMap.get('page')
    this.service.getQuestion(this.id,this.page).subscribe(data=>{
      this.question=data
    });
    
  }

}
