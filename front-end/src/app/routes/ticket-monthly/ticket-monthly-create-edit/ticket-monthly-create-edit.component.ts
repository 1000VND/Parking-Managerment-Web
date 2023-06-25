import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { WebcamImage } from 'ngx-webcam';
import { Observable, Subject, finalize } from 'rxjs';
import { GetAllDataTicketMonthlyDto } from 'src/app/models/TicketMonthly/get-all.model';
import { PromotionService } from 'src/app/services/promotion.service';
import { TicketService } from 'src/app/services/ticket.service';
import { LoadingComponent } from '../../common/loading/loading.component';
import * as Tesseract from 'tesseract.js';
import { differenceInCalendarDays } from 'date-fns';
import { PromotionDtoInput } from 'src/app/models/Promotion/promotion-dto-input';

@Component({
  selector: 'app-ticket-monthly-create-edit',
  templateUrl: './ticket-monthly-create-edit.component.html',
  styleUrls: ['../ticket-monthly.component.css']
})
export class TicketMonthlyCreateEditComponent {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  form!: FormGroup;
  isVisible = false;
  isOkLoading = false;
  options = [
    { label: 'Male', value: 1 },
    { label: 'Female', value: 0 }
  ];
  ticketMonthlyDto: GetAllDataTicketMonthlyDto = new GetAllDataTicketMonthlyDto();
  isCameraExist = true;
  showWebcamIn = true;
  triggerIn: Subject<void> = new Subject<void>();
  webcamImageIn!: WebcamImage | undefined;
  nextWebcamIn: Subject<boolean | string> = new Subject<boolean | string>();
  resultImageIn!: string;
  imageIn: string = '/assets/cameranotfound.png';
  isScan: boolean = true;
  statusCar: string = '';
  today = new Date()
  voucher: number = -1;
  disabledDate = (current: Date): boolean => differenceInCalendarDays(current, this.today) < 0;
  listPromoData: { label: string, value: number }[] = [];
  ticketDto: any;
  price: number = 0;
  discount: number = 0;
  disableInput = true;
  btnOk: boolean = false;

  constructor(
    private _service: TicketService,
    private _promoService: PromotionService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [null],
      licensePlate: [{ value: this.resultImageIn, disabled: true }, Validators.required],
      customerImage: [null, Validators.required],
      customerName: [null, Validators.required],
      phoneNumber: [null, Validators.required],
      customerAddress: [null, Validators.required],
      birthDay: [null,],
      gender: [null, Validators.required],
      lastRegisterDate: [null, Validators.required],
      promotionId: [null],
      cost: [{ disabled: true, value: this.price }, Validators.required],
      discount: [{ disabled: true, value: 0 }]
    });
  }

  handleCancel() {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save(): void {
    if (this.form.valid) {
      this.isOkLoading = true;
      this.ticketDto = Object.assign({}, this.form.value);
      this.ticketDto.licensePlate = this.form.controls['licensePlate'].value;
      this.ticketDto.cost = this.form.controls['cost'].value;
      this.validateForm(this.form);
      this._service.createOrEdit(this.ticketDto).pipe(finalize(() => {
        this.refresh();
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
      this.toastr.warning('You have not entered enough information!')
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  refresh() {
    this.voucher = 0;
    this.listPromoData = [];
    this.discount = 0;
    this.price = 0;
  }

  show(dto?: GetAllDataTicketMonthlyDto) {
    this.refresh();
    this.form.reset();
    if (dto) {
      this.btnOk = true
      this.form.patchValue(dto);
      this.ticketMonthlyDto = dto;
      this.imageIn = this.ticketMonthlyDto.customerImgage;
    } else {
      this.ticketMonthlyDto = new GetAllDataTicketMonthlyDto();
      this.btnOk = false
    }
    this.isVisible = true;
  }

  validateForm(form: FormGroup): void {
    for (const i in form.controls) {
      form.controls[i].markAsDirty();
      form.controls[i].updateValueAndValidity();
    }
  }

  triggerObservableIn(): Observable<void> {
    return this.triggerIn.asObservable();
  }

  handleImageIn(webcamImage: WebcamImage) {
    this.webcamImageIn = webcamImage;
  }

  nextWebcamObservableIn(): Observable<boolean | string> {
    return this.nextWebcamIn.asObservable();
  }

  capPictureIn() {
    this.triggerIn.next();
    this.imageIn = this.webcamImageIn!.imageAsDataUrl;
    this.form.controls['customerImage'].patchValue(this.imageIn);
    this.convertImageIn();
  }

  convertImageIn() {
    this.loading.loading(true);
    this.resultImageIn = ''
    this.isScan = true;
    Tesseract.recognize(this.imageIn, 'eng').then(({ data: { text } }) => {
      const noSpecialCharacters = text.replace(/[^a-zA-Z0-9]/g, '');
      const checkPlate = /^\d{2}[A-Za-z]\d*$/.test(noSpecialCharacters.toString());
      if (!(noSpecialCharacters.length == 8)) {
        this.toastr.warning("Invalid license plate!");
        this.isScan = false;
        this.loading.loading(false);
        return
      }
      else {
        if (checkPlate) {
          this.resultImageIn = noSpecialCharacters;
          this.form.controls['licensePlate'].setValue(this.resultImageIn)
          this.form.controls['promotionId'].setValue(this.voucher);
          this._service.checkRegister(this.resultImageIn).pipe(finalize(() => {
            this._promoService.findPromotionByDay(this.resultImageIn).pipe(finalize(() => {
            })).subscribe(res => {
              if (res.data != null) {
                this.listPromoData = (res.data as any[]).map(e => ({
                  value: e.id,
                  label: e.promotionName,
                }));
              } else {
                this.listPromoData = [];
              }
            });
            this.loading.loading(false);
          })).subscribe((res) => {
            this.statusCar = res.message;
          });
        }
        else {
          this.toastr.warning("Invalid license plate!");
          this.isScan = false;
          this.loading.loading(false);
          return
        }
      }
    });
  }

  count() {
    this.loading.loading(true);
    if (this.form.valid) {
      const numMonth = this.form.value.lastRegisterDate.getMonth() == this.today.getMonth() ? 1 :
        ((this.form.value.lastRegisterDate.getMonth() - this.today.getMonth()) + (12 * (this.form.value.lastRegisterDate.getFullYear() - this.today.getFullYear())) + 1);
      this.loading.loading(false);
      this.price = 500000 * numMonth * (100 - this.discount) * 0.01;

    }
    else {
      this.loading.loading(false);
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  onSelectChange(value: any): void {
    if (value != 0) {
      this.loading.loading(true);
      this._promoService.findPromotionById(value).pipe(finalize(() => {
        this.loading.loading(false);
      })).subscribe(res => {
        this.discount = res.data.discount;
      })
    }
  }
}
