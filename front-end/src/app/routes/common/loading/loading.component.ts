import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'parking-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent {

  constructor(
    private spinner: NgxSpinnerService
  ) { }

  loading(loading: boolean) {
    if (loading) this.spinner.show();
    else this.spinner.hide();
  }
}
