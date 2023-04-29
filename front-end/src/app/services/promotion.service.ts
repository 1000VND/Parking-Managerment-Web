import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environment/environment';

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
}

