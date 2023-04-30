import { Component } from '@angular/core';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { addTrack } from 'src/app/models/addTrack';
import { TrackApiService } from 'src/app/api-services/track-api.service';
import { UploadService } from 'src/app/services/upload.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-addtrack',
  templateUrl: './addtrack.component.html',
  styleUrls: ['./addtrack.component.css']
})
export class AddtrackComponent {
  form:FormGroup
  imageUrl:any;
  audioUrl:any;
  constructor(private fb:FormBuilder,private upload:UploadService,private router:Router ,private service: TrackApiService){
  this.form = this.fb.group({
    name: ['',Validators.required]
  })}


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
  addTrack()
  {
    const val = this.form.value;
    let model = <addTrack>{};
    model.title = val.name;
    model.audioFile = this.audioUrl;
    model.logoFile =  this.imageUrl;
    this.service.addTrack(model).subscribe();
    this.audioUrl = null;
    this.imageUrl = null;
    this.router.navigateByUrl('/tracks/1');
  }
}

