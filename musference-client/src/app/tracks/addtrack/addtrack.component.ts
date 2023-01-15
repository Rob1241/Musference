import { Component } from '@angular/core';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { addTrack } from 'src/app/models/addTrack';
import { TrackApiService } from 'src/app/api-services/track-api.service';

@Component({
  selector: 'app-addtrack',
  templateUrl: './addtrack.component.html',
  styleUrls: ['./addtrack.component.css']
})
export class AddtrackComponent {
  form:FormGroup
  constructor(private fb:FormBuilder, private service: TrackApiService){
  this.form = this.fb.group({
    name: ['',Validators.required]
  })}

  addTrack()
  {
    
    const val = this.form.value;
    let model = <addTrack>{};
    model.title = val.name;
    this.service.addTrack(model).subscribe();
    console.log("addtrackworks")
  }
}

