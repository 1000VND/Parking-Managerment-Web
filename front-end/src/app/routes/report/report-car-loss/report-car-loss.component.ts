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
  dataReport: CarReportDto[] = [];
  selectedRowIndex = -1;
  selectedItem: CarReportDto = new CarReportDto();

  constructor(
    private _service: CarReportService,
    private _http: HttpClient,
    private renderer: Renderer2
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

  printReport(selected: number) {
    this.loading.loading(true);
    let today = new Date();
    let dd = String(today.getDate()).padStart(2, '0');
    let mm = String(today.getMonth() + 1).padStart(2, '0');
    let yyyy = today.getFullYear();

    this._service.getDataForByIdCarReport(selected).pipe(finalize(() => {
      if (this.dataReport) {
        setTimeout(()=>{
          const body = {
            userName: this.dataReport[0].userName,
            customerName: this.dataReport[0].customerName,
            customerBirthday: this.dataReport[0].customerBirthday,
            identityCard:this.dataReport[0].identityCard,
            licensePlate: this.dataReport[0].licensePlate,
            customerNumber: this.dataReport[0].customerNumber
          };

  
          return this._http.post(`${environment.baseUrl}Report/CarReport`, body, {
            responseType: "blob",
          }).pipe(finalize(() => {
            this.loading.loading(false);
          })).subscribe((blob) => {
            saveAs(blob, "Biên bản mất xe " + dd + "_" + mm + "_" + yyyy + ".docx");
          });
        },500)
      }
    })).subscribe((res) => {
      this.dataReport = res.data;
    });

  }

  onChangeSelectRow(index: number) {
    if (index === this.selectedRowIndex) {
      this.selectedRowIndex = -1;
      this.selectedItem = new CarReportDto();
    } else {
      const previousRowElement = document.querySelector('.selected');
      if (previousRowElement) {
        this.renderer.removeClass(previousRowElement, 'selected');
      }
      this.selectedRowIndex = index;
      this.selectedItem = this.listData.find(e => e.id == index);
    }

    const rowElements = document.querySelectorAll('tr[nz-tr]');
    rowElements.forEach((rowElement, i) => {
      if (i === this.selectedRowIndex) {
        this.renderer.addClass(rowElement, 'selected');
      } else {
        this.renderer.removeClass(rowElement, 'selected');
      }
    });
  }
}
