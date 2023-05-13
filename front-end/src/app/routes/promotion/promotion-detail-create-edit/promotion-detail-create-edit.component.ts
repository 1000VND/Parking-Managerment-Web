import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { CreateEditPromotionDetail } from 'src/app/models/Promotion/create-edit-promotion-detail';
import { PromotionService } from 'src/app/services/promotion.service';
import { TicketService } from 'src/app/services/ticket.service';
import { LoadingComponent } from '../../common/loading/loading.component';
import { GetAllDataPromotionDto } from 'src/app/models/Promotion/get-all.model';

@Component({
  selector: 'promotion-detail-create-edit',
  templateUrl: './promotion-detail-create-edit.component.html',
  styleUrls: ['../promotion.component.css']
})
export class PromotionDetailCreateEditComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  form!: FormGroup;
  isVisible = false;
  isOkLoading = false;
  promotion: { label: string, value: number } = { label: '', value: 0 };
  userList: { label: string, value: number }[] = [];
  promotionDetailDto: CreateEditPromotionDetail = new CreateEditPromotionDetail();


  constructor(
    private _service: PromotionService,
    private _tiketService: TicketService,
    private fb: FormBuilder,
    private _toastr: ToastrService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      id: [null],
      promotionName: [null, Validators.required],
      user: [null, Validators.required],
    })

  }

  cancel() {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save() {
    this.promotionDetailDto.promotionId = this.promotion.value;
    this.promotionDetailDto.userId = this.form.value.user;
    console.log(this.promotionDetailDto);
    if (this.form.valid) {
      this.isOkLoading = true;
      this._service.createEditPromotionDetail(this.promotionDetailDto).pipe(finalize(() => {
        this.isVisible = false;
        this.isOkLoading = false;
        this.modalSave.emit(null);
      })).subscribe((res) => {
        if (res.statusCode == 200) {
          this._toastr.success(res.message);
        }
      })
    }
    else {
      this._toastr.error('You have not entered enough information!')
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  getUser() {
    this.userList = [];
    this.loading.loading(true);
    this._tiketService.getAllTicket().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe(res => {
      res.data.map((e: any) => {
        this.userList.push({
          label: e.licensePlate,
          value: e.id,
        });
      });
    })
  }


  show(data: GetAllDataPromotionDto) {
    this.getUser();
    this.form.reset();
    if (data) {
      this.promotion = {
        label: data.promotionName,
        value: data.id
      }
    } else {
      this.promotionDetailDto = new CreateEditPromotionDetail();
    }
    this.isVisible = true;
  }

}
