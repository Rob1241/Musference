import { Component, OnInit } from '@angular/core';
import { UserApiService } from '../api-services/user-api.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  id:any ;
  user:any;
  constructor(private service: UserApiService,private router:Router, private activatedRoute:ActivatedRoute) { }

  

  isowner(){
    if(this.activatedRoute.snapshot.paramMap.get('id')==localStorage.getItem('user_id'))
    {return true}
    else return false;
  }

  route() {
    this.router.navigateByUrl('/edit-profile');
    }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.paramMap.get('id')
    this.service.getUser(this.id).subscribe(data=>{
      this.user = data;
      
    console.log(this.activatedRoute.snapshot.paramMap.get('id'));
    console.log(localStorage.getItem('user_id'));
    })
  }

}
