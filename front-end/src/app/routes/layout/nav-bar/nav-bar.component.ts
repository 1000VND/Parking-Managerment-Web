import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  userLocal: any;
  userSession: any;
  isCollapsed = false;
  isPermission = false;
  routerLink!: string;
  constructor(
    private router: Router
  ) { }
  ngOnInit(): void {
    this.userLocal = JSON.parse(localStorage.getItem('user') || '{}').fullName;
    this.userSession = JSON.parse(sessionStorage.getItem("user") || '{}').role;
    //localStorage.removeItem('user')
  }

  submitForm() {
    this.router.navigateByUrl("login");
  }

  toggleCollapsed(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  logout() {
    localStorage.removeItem('user');
  }

}
