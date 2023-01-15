import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrackApiService {

  readonly trackAPIUrl = "https://localhost:7289/api"
  constructor(private http:HttpClient) { }

  getAllTracksNewest(page:number):Observable<any> {  
    return this.http.get<any>(this.trackAPIUrl+`/Track/${page}`);
  }
  getAllTracksMostLiked(page:number):Observable<any> {  
    return this.http.get<any>(this.trackAPIUrl+`/Track/most_liked/${page}`);
  }
  addTrack(data:any) {
    return this.http.post<any>(this.trackAPIUrl+'/Track',data);
  }
  deleteTrack(id:Number) {  
    return this.http.delete<any>(this.trackAPIUrl+`/Track/${id}`);
  }
  likeTrack(id:Number) {
    return this.http.put<any>(this.trackAPIUrl+`/Track/${id}/LikeTrack`,null);
  }
  searchTrack(text:Text, page:number){
      return this.http.get<any>(this.trackAPIUrl+`/Track/Search/${page}/${text}`);
  }
}
 