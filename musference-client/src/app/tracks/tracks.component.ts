import { Component, OnInit } from '@angular/core';
import { TrackApiService } from '../api-services/track-api.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { searchTrackModel } from '../models/searchTrackModel';

@Component({
  selector: 'app-tracks',
  templateUrl: './tracks.component.html',
  styleUrls: ['./tracks.component.css']
})
export class TracksComponent implements OnInit {
  pageid:any;
  tracks:any;
  searchedtracks:any;
  form:FormGroup;
  constructor(private service:TrackApiService, 
    private activatedRoute:ActivatedRoute
    ,private fb:FormBuilder) {
      this.form=this.fb.group({
        search: ['',Validators.required]});
     }

  likeTrack(track_id:Number){
    this.service.likeTrack(track_id).subscribe();
  }
  deleteTrack(track_id:Number){
    this.service.deleteTrack(track_id).subscribe();
    
  }
  searchTracks(){
    const val = this.form.value;
    let model = <searchTrackModel>{};
    model.text = val.search; 
    this.service.searchTrack(val.search,1).subscribe(data=>{
      this.searchedtracks=data
    })
  }
  isOwner(id:Number){
    if(Number(localStorage.getItem('user_id'))==id)return true
    else return false;
  }
  ngOnInit(): void {
    this.pageid = this.activatedRoute.snapshot.paramMap.get('page')
    this.service.getAllTracksNewest(this.pageid).subscribe(data=>{
      this.tracks=data;
      console.log(data)
    })
  }

}
