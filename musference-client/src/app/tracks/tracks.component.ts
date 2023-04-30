import { Component, OnInit } from '@angular/core';
import { TrackApiService } from '../api-services/track-api.service';
import { Router,ActivatedRoute } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '../api-services/auth.service';
// import * as Howl from 'ngx-howler';

export interface Track {
  name:string;
  path:string;
}

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
  params:any;
  functionParam:any;
  searchParam:any;
  audio:any;
  time:any = 0;

  constructor(private service:TrackApiService,private authService: AuthService,
    private activatedRoute:ActivatedRoute,private router:Router,
    private fb:FormBuilder) {
      this.form=this.fb.group({
        search: ['',Validators.required]});
      this.activatedRoute.queryParams.subscribe(data=>{
          this.params = data;
          this.functionParam=this.params.function;
          this.searchParam=this.params.search;
        })
     }
    playSound(source:any){
      this.audio = new Audio();
      this.audio.src = source;
      console.log(source)
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
    ifNewest(){
      if(this.tracks.result.pages==1)return false;
       if(typeof this.functionParam === 'undefined')return true;
       else return false;
    }
    pagValue(){
      if(this.tracks.result.currentPage==1){
        return 0;
      }
      if(this.tracks.result.currentPage>1&&this.tracks.result.currentPage<this.tracks.result.pages){
        return 1;
      }
      else {
        return 2;
      }
    }
    next(){
      this.router.navigateByUrl(`/tracks/${this.tracks.result.currentPage+1}`);
    }
    previous(){
      this.router.navigateByUrl(`/tracks/${this.tracks.result.currentPage-1}`);
    }
  likeTrack(track_id:Number){
    if(this.authService.isLoggedIn()){
    this.service.likeTrack(track_id).subscribe();
    window.location.reload();
    }
  }
  deleteTrack(track_id:Number){
    this.service.deleteTrack(track_id).subscribe();
    window.location.reload();
  }
  searchTracks(){
    const val = this.form.value;
    this.router.navigate(['/tracks'],{queryParams:{function:'search',search:val.search}})
  }
  mostLiked(){
    this.router.navigate(['/tracks'],{queryParams:{function:'mostliked'}})
  }
  isOwner(id:Number){
    if(Number(localStorage.getItem('user_id'))==id&&this.authService.isLoggedIn())return true
    else return false;
  }
  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.pageid = this.activatedRoute.snapshot.paramMap.get('page')
    if(this.functionParam=='mostliked')
    {
      this.service.getAllTracksMostLiked(1).subscribe(data=>{
        this.tracks=data;
      })
    }
    if(this.functionParam=='search')
    {
      this.service.searchTrack(this.searchParam,1).subscribe(data=>{
        this.tracks=data;
      })
    }else
    this.service.getAllTracksNewest(this.pageid).subscribe(data=>{
      this.tracks=data;
    })
  }

}
