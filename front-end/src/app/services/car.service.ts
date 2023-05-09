import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { CarInput } from '../models/Car/car-input.model';
import { CarOutDto } from '../models/Car/car-out-dto.model';

@Injectable({
  providedIn: 'root'
})
export class CarService {

  baseUrl = environment.baseUrl + "Car/";

  constructor(
    private _http: HttpClient,
    private _toastr: ToastrService
  ) { }

  checkTypeCustomer(par: string): Observable<any> {
    return this._http.post(this.baseUrl + 'CheckTypeCustomer?plate=' + par, null).pipe(catchError((err) => {
      return of(err);
    }));
  }

  takeCar(input: CarInput): Observable<any> {
    return this._http.post(this.baseUrl + 'CarIn', input).pipe(catchError((err) => {
      this._toastr.error(err.error.message);
      return of(err);
    }))
  }

  checkLicensePlate(plate: string): Observable<any> {
    return this._http.get(this.baseUrl + 'CheckLPCarOut?plate=' + plate).pipe(catchError((err) => {
      return of(err);
    }));
  }

  carOut(input: CarOutDto): Observable<any>{
    return this._http.post(this.baseUrl + 'CarOut', input).pipe(catchError((err) => {
      return of(err);
    }));
  }
}
