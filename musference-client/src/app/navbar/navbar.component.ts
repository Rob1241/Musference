import { Component, OnInit } from '@angular/core';
import { AuthService } from '../api-services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private service: AuthService,private router:Router) { }
  
  userid = localStorage.getItem('user_id');
  login(){
    this.router.navigate(['/login']);
  }
  account(){
    this.router.navigate([`/account/${this.userid}`]);
  }
  tousers(){
    this.router.navigate(['/users/1']);
  }
  signup(){
    this.router.navigate(['/signup']);
  }

  isLoggedIn = this.service.isLoggedIn();  
  isLoggedOut = this.service.isLoggedOut();  
  logout(){
    this.service.logout();
    this.isLoggedIn = false;
    this.isLoggedOut = true;
    this.router.navigate(['/']);
  }
  ngOnInit(): void {
    this.service.isLoggedInUser.subscribe((data)=>{
      if(data){
        this.isLoggedIn = true;
        this.isLoggedOut = false;
      }
    })
  }

}
