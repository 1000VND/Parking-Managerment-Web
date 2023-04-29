import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { UserDto } from '../models/user.model';
import { Observable, catchError, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CreateEditUserDto } from '../models/User/createedit.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.baseUrl + 'User/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
  ) { }

  getAllUser(): Observable<any> {
    return this.http.get<UserDto>(this.baseUrl + 'GetAll').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  createOrEdit(data: CreateEditUserDto): Observable<any> {
    return this.http.post(this.baseUrl + 'CreateOrEdit', data).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'Delete/' + id).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }
}
