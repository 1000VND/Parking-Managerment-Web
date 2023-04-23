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
import { CarComponent } from './routes/car/car.component';
import { WebcamModule } from 'ngx-webcam';
import { NgxSpinnerModule } from "ngx-spinner";
import { TicketMonthlyComponent } from './routes/ticket-monthly/ticket-monthly.component';
import { LoadingComponent } from './routes/common/loading/loading.component';
import { NgOptimizedImage } from '@angular/common';
import { NgZorroAntdModule } from './ng-zorro-antd/ng-zorro-antd/ng-zorro-antd.module';
import { UserComponent } from './routes/user/user.component';
import { CreateEditUserComponent } from './routes/user/create-edit-user/create-edit-user.component';

@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    LoginComponent,
    HomeComponent,
    NavBarComponent,
    CarComponent,
    UserComponent,
    CreateEditUserComponent,
    TicketMonthlyComponent,
    TicketMonthlyComponent,
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
    NgxSpinnerModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
