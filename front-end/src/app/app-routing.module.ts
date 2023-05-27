import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './routes/home/home.component';
import { LoginComponent } from './routes/login/login.component';
import { NavBarComponent } from './routes/layout/nav-bar/nav-bar.component';
import { TicketMonthlyComponent } from './routes/ticket-monthly/ticket-monthly.component';
import { UserComponent } from './routes/user/user.component';
import { PromotionComponent } from './routes/promotion/promotion.component';
import { ReportCarLossComponent } from './routes/report/report-car-loss/report-car-loss.component';
const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/login' },
  { path: 'login', component: LoginComponent },
  {
    path: 'nav-bar', component: NavBarComponent, children: [
      // Role User
      { path: 'home', component: HomeComponent },
      { path: 'ticket-monthly', component: TicketMonthlyComponent },
      { path: 'report-car-loss', component: ReportCarLossComponent },

      // Role amdin
      { path: 'register', component: UserComponent },
      { path: 'promotion', component: PromotionComponent },
    ]
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
