import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { CarReportDto } from 'src/app/models/Report/CarReport/car-report.model';
import { ToastrService } from 'ngx-toastr';
import { LoadingComponent } from 'src/app/routes/common/loading/loading.component';
import { finalize } from 'rxjs';
import { CarReportService } from 'src/app/services/car-report.service';
import { HttpClient } from '@angular/common/http';
import * as saveAs from 'file-saver';
import { environment } from 'src/environment/environment';

@Component({
  selector: 'report-car-loss',
  templateUrl: './report-car-loss.component.html',
  styleUrls: ['./report-car-loss.component.css']
})
export class ReportCarLossComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  listData: any[] = [];
  isVisible = false;
  isOkLoading = false;
  selectedData: any;
  addCarReport: CarReportDto = new CarReportDto();
  selectedRowIndex = -1;

  constructor(
    private _service: CarReportService,
    private _toast: ToastrService,
    private _http: HttpClient
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.getAllData();
  }

  getAllData() {
    this.listData = [];
    this.loading.loading(true);
    this._service.getAllCarReport().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe((res) => {
      this.listData = res.data;
    });
  }

  printReport() {
    const body = {
      day: 3,
      month: 5,
      year: 2023
    }
    return this._http
      .post(
        `${environment.baseUrl}Report/CarReport`,
        body,
        {
          responseType: "blob",
        }
      )
      .pipe(finalize(() => (this.loading.loading(false))))
      .subscribe((blob) => {
        saveAs(blob, "hungw" + ".docx");
      });
  }

}
