import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { Login } from '../models/User/login.model';
import { UserDto } from '../models/User/user.model';
import { ToastrService } from 'ngx-toastr';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  baseUrl = environment.baseUrl + 'Login/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  checkUser(dto: Login) {
    if (dto) {
      return this.http.get(this.baseUrl + 'login?UserName=' + dto.userName + '&PassWord=' + dto.passWord).pipe(catchError((err) => {
        this.toastr.error(err.error.message);
        return of(err);
      }));
    }
  }
}

