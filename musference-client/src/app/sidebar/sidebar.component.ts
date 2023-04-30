
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private router:Router) { }

  toquestion(){
    this.router.navigate(['/questions/1']);
  }
  totracks(){
    this.router.navigate(['/tracks/1']);
  }
  ngOnInit(): void {
  }

}
