import { Component, OnInit } from '@angular/core';
import { UserApiService } from '../api-services/user-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  form:FormGroup;
  page:any;
  users:any;
  params:any;
  functionParam:any;
  searchParam:any;
  constructor(private service:UserApiService,private router:Router,private activatedRoute:ActivatedRoute,private fb:FormBuilder ) { 
    this.form = this.fb.group({
      search: ['',Validators.required]});
    this.activatedRoute.queryParams.subscribe(data=>{
        this.params = data;
        this.functionParam=this.params.function;
        this.searchParam=this.params.search;
      })
  }
  ifNewest(){
    if(this.users.result.pages==1)return false;
     if(typeof this.functionParam === 'undefined')return true;
     else return false;
  }

  pagValue(){
    if(this.users.result.currentPage==1){
      return 0;
    }
    if(this.users.result.currentPage>1&&this.users.result.currentPage<this.users.result.pages){
      return 1;
    }
    else {
      return 2;
    }
  }
  next(){
    this.router.navigateByUrl(`/users/${this.users.result.currentPage+1}`);
  }
  previous(){
    this.router.navigateByUrl(`/users/${this.users.result.currentPage-1}`);
  }
  searchTracks(){
    const val = this.form.value;
    this.router.navigate(['/users'],{queryParams:{function:'search',search:val.search}})
  }
  mostLiked(){
    this.router.navigate(['/users'],{queryParams:{function:'reputation'}})
  }
  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.page = this.activatedRoute.snapshot.paramMap.get('page')
    if(this.functionParam=='reputation')
    {
      this.service.getAllUsersReputation(1).subscribe(data=>{
        this.users=data;
      })
    }
    if(this.functionParam=='search')
    {
      this.service.searchUsers(1, this.searchParam).subscribe(data=>{
        this.users=data;
      })
    }
    this.service.getAllUsersNewest(this.page).subscribe(data=>{
      this.users=data;
    });
  }

}
