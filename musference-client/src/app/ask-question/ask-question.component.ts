import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionApiService } from '../api-services/question-api.service';
import { AddQuestionModel } from '../models/question-model';
import { UploadService } from '../services/upload.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ask-question',
  templateUrl: './ask-question.component.html',
  styleUrls: ['./ask-question.component.css']
})
export class AskQuestionComponent implements OnInit {
  form:FormGroup
  imageUrl:any;
  audioUrl:any;
  constructor(private fb:FormBuilder,private upload:UploadService,private service:QuestionApiService,private router:Router) { 
    this.form= this.fb.group({
      heading: ['',Validators.required],
      content: ['',Validators.required]
  });
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

  addQuestion(){
  const val =this.form.value;
    let model = <AddQuestionModel>{};
    model.heading = val.heading;
    model.content = val.content;
    model.audioFile = this.audioUrl;
    model.imageFile =  this.imageUrl;
    this.service.addQuestion(model).subscribe();
    this.audioUrl = null;
    this.imageUrl = null;
    this.router.navigateByUrl('/questions/1');
  }
  ngOnInit(): void {

    
  }
}


