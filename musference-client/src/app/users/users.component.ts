import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { UserApiService } from '../user-api.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  userList$!:Observable<any[]>;

  constructor(private service:UserApiService) { }

  ngOnInit(): void {
    this.userList$ = this.service.getAllUsers();
  }

}
