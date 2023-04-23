import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './routes/home/home.component';
import { LoginComponent } from './routes/login/login.component';
import { CarComponent } from './routes/car/car.component';
import { NavBarComponent } from './routes/layout/nav-bar/nav-bar.component';
import { TicketMonthlyComponent } from './routes/ticket-monthly/ticket-monthly.component';
import { UserComponent } from './routes/user/user.component';
const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/login' },
  { path: 'login', component: LoginComponent },
  {
    path: 'nav-bar', component: NavBarComponent, children: [
      { path: 'home', component: HomeComponent },
      { path: 'register', component: UserComponent },
      { path: 'ticket-monthly', component: TicketMonthlyComponent },
      { path: 'car', component: CarComponent }
    ]
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
