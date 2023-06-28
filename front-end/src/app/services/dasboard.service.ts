import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class DasboardService {

  baseUrl = environment.baseUrl + 'Dashboard/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  countAllTicketMonthly(): Observable<any> {
    return this.http.get(this.baseUrl + 'CountAllTicketMonthly').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  rateMaleFemale(): Observable<any> {
    return this.http.get(this.baseUrl + 'RateMaleFemale').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  getDataCard(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetDataCard').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }
}
