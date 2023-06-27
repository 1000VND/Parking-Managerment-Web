import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CarReportService } from 'src/app/services/car-report.service';
import { CreateCarReportDto } from 'src/app/models/Report/CarReport/create-car-report.model';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'report-car-loss-create',
  templateUrl: './report-car-loss-create.component.html',
  styleUrls: ['../report-car-loss.component.css']
})
export class CarReportCreateComponent implements OnInit {
  userSession: any;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  isVisible = false;
  isOkLoading = false;
  options = [
    { label: 'Xe bị mất', value: 'Xe bị mất' },
    { label: 'Xe bị hỏng', value: 'Xe bị hỏng' }
  ];
  userLocal = JSON.parse(localStorage.getItem('user') || '{}').fullName;
  carReportDto: CreateCarReportDto = new CreateCarReportDto();
  form!: FormGroup;

  constructor(
    private _service: CarReportService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.userSession = JSON.parse(sessionStorage.getItem("user") || '{}').id;

    this.form = this.fb.group({
      id: [null],
      customerName: [null, Validators.required],
      userName: [null],
      licensePlate: [null, Validators.required],
      reason: [null, Validators.required],
      content: [null, Validators.required],
    })
  }

  handleCancel(): void {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save(): void {
    if (this.form.valid) {
      this.isOkLoading = true;
      this.validateForm(this.form);
      this.carReportDto = Object.assign({}, this.form.value);
      this.carReportDto.userId = this.userSession;

      this._service.create(this.carReportDto).pipe(finalize(() => {
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

  show(carReportDto?: CreateCarReportDto) {
    this.form.reset();
    this.form.controls['userName'].setValue(this.userLocal);

    if (carReportDto) {
      this.form.patchValue(carReportDto);
      this.carReportDto = carReportDto;
    } else {
      this.carReportDto = new CreateCarReportDto();

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
