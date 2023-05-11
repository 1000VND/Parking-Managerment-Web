import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { PromotionService } from 'src/app/services/promotion.service';
import { LoadingComponent } from '../common/loading/loading.component';
import { UntypedFormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { PromotionDtoInput } from 'src/app/models/Promotion/promotion-dto-input';
import { GetAllDataPromotionDto } from 'src/app/models/Promotion/get-all.model';

@Component({
  selector: 'app-promotion',
  templateUrl: './promotion.component.html',
  styleUrls: ['./promotion.component.css']
})
export class PromotionComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;

  listData: any[] = [];
  plate: string = '';
  selectedRowIndex = -1;
  selectedItem: GetAllDataPromotionDto = new GetAllDataPromotionDto();
  searchInput: PromotionDtoInput = new PromotionDtoInput();
  dataSubTale: any[] = [];

  constructor(
    private _service: PromotionService,
    private _toastr: ToastrService,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this._service.getAllPromotion(this.searchInput).pipe(finalize(() => {
    })).subscribe(res => {
      this.listData = res.data;
    })
  }

  deletePromotion(id: number) {
    this.loading.loading(true);
    this._service.deletePromotion(id)
      .pipe(
        finalize(() => {
          this.loading.loading(false);
        })).subscribe((res) => {
          if (res.statusCode == 200) {
            this._toastr.success('Delete success');
          }
          this.listData = this.listData.filter((user) => user.id !== id);
        });
  }

  onChangeSelectRow(index: number) {
    if (index === this.selectedRowIndex) {
      this.selectedRowIndex = -1;
      this.selectedItem = new GetAllDataPromotionDto();
    } else {
      const previousRowElement = document.querySelector('.selected');
      if (previousRowElement) {
        this.renderer.removeClass(previousRowElement, 'selected');
      }
      this.selectedRowIndex = index;
      this.selectedItem = this.listData.find(e => e.id == index);
      console.log(this.selectedItem)
    }

    const rowElements = document.querySelectorAll('tr[nz-tr]');
    rowElements.forEach((rowElement, i) => {
      if (i === this.selectedRowIndex) {
        this.renderer.addClass(rowElement, 'selected');
      } else {
        this.renderer.removeClass(rowElement, 'selected');
      }
    });
    this.getPromotionDetail();
  }

  getPromotionDetail() {
    this.loading.loading(true);
    this._service.getPromotionDetail(this.selectedItem.id).pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      this.dataSubTale = [...res.data];
    })
  }
}
