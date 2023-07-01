import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TableModule } from './modules/table/table.module';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent } from './routes/login/login.component';
import { HomeComponent } from './routes/home/home.component';
import { NavBarComponent } from './routes/layout/nav-bar/nav-bar.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { WebcamModule } from 'ngx-webcam';
import { NgxSpinnerModule } from "ngx-spinner";
import { TicketMonthlyComponent } from './routes/ticket-monthly/ticket-monthly.component';
import { LoadingComponent } from './routes/common/loading/loading.component';
import { NgOptimizedImage } from '@angular/common';
import { NgZorroAntdModule } from './ng-zorro-antd/ng-zorro-antd/ng-zorro-antd.module';
import { UserComponent } from './routes/user/user.component';
import { UserCreateEditComponent } from './routes/user/user-create-edit/user-create-edit.component';
import { PromotionComponent } from './routes/promotion/promotion.component';
import { PromotionCreateEditComponent } from './routes/promotion/promotion-create-edit/promotion-create-edit.component';
import { PromotionDetailCreateEditComponent } from './routes/promotion/promotion-detail-create-edit/promotion-detail-create-edit.component';
import { ReportCarLossComponent } from './routes/report/report-car-loss/report-car-loss.component';
import { CarReportCreateComponent } from './routes/report/report-car-loss/report-car-loss-create/report-car-loss-create.component';
import { TicketMonthlyCreateEditComponent } from './routes/ticket-monthly/ticket-monthly-create-edit/ticket-monthly-create-edit.component';
import { DxChartModule, DxPieChartModule } from 'devextreme-angular';
import { DashboardComponent } from './routes/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    LoginComponent,
    HomeComponent,
    NavBarComponent,
    UserComponent,
    TicketMonthlyComponent,
    UserCreateEditComponent,
    PromotionComponent,
    PromotionCreateEditComponent,
    PromotionDetailCreateEditComponent,
    ReportCarLossComponent,
    CarReportCreateComponent,
    TicketMonthlyCreateEditComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    TableModule,
    ReactiveFormsModule,
    CommonModule,
    ToastrModule.forRoot(),
    DragDropModule,
    ScrollingModule,
    WebcamModule,
    NgOptimizedImage,
    NgZorroAntdModule,
    NgxSpinnerModule,
    DxChartModule,
    DxPieChartModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
