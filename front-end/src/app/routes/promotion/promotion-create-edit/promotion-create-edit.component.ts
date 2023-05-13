import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { GetAllDataPromotionDto } from 'src/app/models/Promotion/get-all.model';
import { PromotionService } from 'src/app/services/promotion.service';

@Component({
  selector: 'promotion-create-edit',
  templateUrl: './promotion-create-edit.component.html',
  styleUrls: ['../promotion.component.css']
})
export class PromotionCreateEditComponent implements OnInit {
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  form!: FormGroup;
  isVisible = false;
  isOkLoading = false;
  promotionDto: GetAllDataPromotionDto = new GetAllDataPromotionDto();

  constructor(
    private _service: PromotionService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [null],
      promotionName: [null, Validators.required],
      fromDate: [null, Validators.required],
      toDate: [null, Validators.required],
      discount: [null, Validators.required],
      point: [null, Validators.required],
    })
  }

  cancel() {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save() {
    if (this.form.valid) {
      this.isOkLoading = true;
      this._service.createOrEdit(this.form.value).pipe(finalize(() => {
        this.isVisible = false;
        this.isOkLoading = false;
        this.modalSave.emit(null);
      })).subscribe((res) => {
        if (res.statusCode == 200) {
          this.toastr.success(res.message);
        }
      })
    }
    else {
      this.toastr.error('You have not entered enough information!')
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  show(dto?: GetAllDataPromotionDto) {
    this.form.reset();
    if (dto) {
      this.form.patchValue(dto);
      this.promotionDto = dto;
    } else {
      this.promotionDto = new GetAllDataPromotionDto();
    }
    this.isVisible = true;
  }

  validateForm(form: FormGroup): void {
    for (const i in form.controls) {
      form.controls[i].markAsDirty();
      form.controls[i].updateValueAndValidity();
    }
  }

}
