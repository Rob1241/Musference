import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UserApiService } from '../api-services/user-api.service';
import { ActivatedRoute } from '@angular/router';
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
  constructor(private service:UserApiService,private activatedRoute:ActivatedRoute,private fb:FormBuilder ) { 
    this.form = this.fb.group({
      search: ['',Validators.required]
    })
  }

  ngOnInit(): void {
    this.page = this.activatedRoute.snapshot.paramMap.get('page')
    this.service.getAllUsersNewest(this.page).subscribe(data=>{
      this.users=data;
    });
  }

}
