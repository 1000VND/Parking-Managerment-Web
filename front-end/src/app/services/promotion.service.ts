import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { CreateEditPromotion } from '../models/Promotion/craete-edit.model';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {

  baseUrl = environment.baseUrl + 'Promotion/';

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
  ) { }

  getAllPromotion(): Observable<any> {
    return this.http.get(this.baseUrl + 'GetAll').pipe(catchError((err) => {
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
}

