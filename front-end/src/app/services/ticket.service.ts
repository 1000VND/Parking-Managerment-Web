import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { CreateEditTicketMonthly } from '../models/TicketMonthly/create-edit.model';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  baseUrl = environment.baseUrl + 'TicketMonthly/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
  ) { }

  getAllTicket(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAll').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
}
createOrEdit(data: CreateEditTicketMonthly): Observable<any> {
  return this.http.post(this.baseUrl + 'CreateOrEdit', data).pipe(catchError((err) => {
    this.toastr.error(err.error.message);
    return of(err);
  }));
}

deleteTicketMonthly(id: number): Observable<any> {
  return this.http.delete(this.baseUrl + `${id}`).pipe(catchError((err) => {
    this.toastr.error(err.error.message);
    return of(err);
  }));
}
}

