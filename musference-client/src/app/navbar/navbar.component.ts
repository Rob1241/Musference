import { Component, OnInit } from '@angular/core';
import { AuthService } from '../api-services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private service: AuthService) { }

  isLoggedIn = this.service.isLoggedIn();  
  isLoggedOut = this.service.isLoggedOut();  
  logout(){
    this.service.logout();
  }
  userid = localStorage.getItem('user_id');
  ngOnInit(): void {
    
  }

}
