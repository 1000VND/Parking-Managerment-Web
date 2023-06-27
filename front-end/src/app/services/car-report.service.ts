import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment';
import { CarReportDto } from '../models/Report/CarReport/car-report.model';
import { Observable, catchError, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CreateCarReportDto } from '../models/Report/CarReport/create-car-report.model';

@Injectable({
  providedIn: 'root'
})
export class CarReportService {

  baseUrl = environment.baseUrl + 'CarReport/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
  ) { }

  getAllCarReport(): Observable<any> {
    return this.http.get<CarReportDto>(this.baseUrl + 'GetAll').pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  create(data: any): Observable<any> {
    return this.http.post(this.baseUrl + 'CreateOrEdit', data).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }
}
