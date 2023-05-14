import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { GetAllDataTicketMonthlyDto } from 'src/app/models/TicketMonthly/get-all.model';
import { PromotionService } from 'src/app/services/promotion.service';
import { TicketService } from 'src/app/services/ticket.service';

@Component({
  selector: 'app-ticket-monthly-create-edit',
  templateUrl: './ticket-monthly-create-edit.component.html',
  styleUrls: ['../ticket-monthly.component.css']
})
export class TicketMonthlyCreateEditComponent {
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  form!: FormGroup;
  isVisible = false;
  isOkLoading = false;
  options = [
    { label: 'Nam', value: 1 },
    { label: 'Nữ', value: 0 }
  ];

  ticketMonthlyDto: GetAllDataTicketMonthlyDto = new GetAllDataTicketMonthlyDto();


  constructor(
    private _service: TicketService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [null],
      licensePlate: [null, Validators.required],
      customerImage: [null, Validators.required],
      customerName: [null, Validators.required],
      phoneNumber: [null, Validators.required],
      customerAddress: [null, Validators.required],
      birthDay: [null, Validators.required],
      gender: [null, Validators.required],
      lastRegisterDate: [null, Validators.required],
      customerPoint: [null, Validators.required],

    })
  }

  handleCancel() {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save(): void {


    if (this.form.valid ) {
      this.isOkLoading = true;
      this.validateForm(this.form);
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


  show(dto?: GetAllDataTicketMonthlyDto) {
    this.form.reset();
    if (dto) {
      this.form.patchValue(dto);
      this.ticketMonthlyDto = dto;
    } else {
      this.ticketMonthlyDto = new GetAllDataTicketMonthlyDto();
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
