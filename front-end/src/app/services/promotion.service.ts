import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { CreateEditPromotion } from '../models/Promotion/create-edit.model';
import { PromotionDtoInput } from '../models/Promotion/promotion-dto-input';
import { CreateEditPromotionDetail } from '../models/Promotion/create-edit-promotion-detail';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {

  baseUrl = environment.baseUrl + 'Promotion/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
  ) { }

  getAllPromotion(data: PromotionDtoInput): Observable<any> {
    return this.http.post(this.baseUrl + 'GetAll', data).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
  }

  getPromotionDetail(id: number): Observable<any> {
    return this.http.get(this.baseUrl + 'PromotionDetail?id=' + id).pipe(catchError(err => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
  }

  createOrEdit(data: CreateEditPromotion): Observable<any> {
    return this.http.post(this.baseUrl + 'CreateOrEdit', data).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  deletePromotion(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `${id}`).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  createEditPromotionDetail(data: CreateEditPromotionDetail): Observable<any> {
    return this.http.post(this.baseUrl + 'CreateEditPromoDetail', data).pipe(catchError(err => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
  }

  deletePromotionDetail(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'DeletePromotionDetail?id=' + id).pipe(catchError((err) => {
      this.toastr.error(err.error.message);
      return of(err);
    }));
  }

  findPromotionById(id: number): Observable<any> {
    return this.http.get(this.baseUrl + "GetDataById?id=" + id).pipe(catchError(err => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
  }

  findPromotionByDay(plate: string): Observable<any> {
    return this.http.get(this.baseUrl + "GetPromotionByNow?plate=" + plate).pipe(catchError(err => {
      this.toastr.error(err.error.message);
      return of(err);
    }))
  }

}

