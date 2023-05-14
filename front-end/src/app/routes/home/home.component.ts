import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { WebcamImage, WebcamInitError, WebcamMirrorProperties, WebcamUtil } from 'ngx-webcam';
import { Observable, Subject, finalize } from 'rxjs';
import { DataFormatService } from 'src/app/services/data-format.service';
import * as Tesseract from 'tesseract.js';
import { LoadingComponent } from '../common/loading/loading.component';
import { CarService } from 'src/app/services/car.service';
import { CarInput } from 'src/app/models/Car/car-input.model';
import { ToastrService } from 'ngx-toastr';
import { CarOutDto } from 'src/app/models/Car/car-out-dto.model';


@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  isCollapsed = false;
  isCameraExist = true;
  showWebcamIn = true;
  showWebcamOut = true;
  webcamImageIn!: WebcamImage | undefined;
  webcamImageOut!: WebcamImage | undefined;
  triggerIn: Subject<void> = new Subject<void>();
  triggerOut: Subject<void> = new Subject<void>();
  nextWebcamIn: Subject<boolean | string> = new Subject<boolean | string>();
  nextWebcamOut: Subject<boolean | string> = new Subject<boolean | string>();
  flippedImage: any;
  imageIn: string = 'assets/error.png';
  imageOut: string = 'assets/error.png';
  resultImageIn!: string;
  resultImageOut: string | undefined;
  maGuixe: string | undefined;
  isScan: boolean = true;
  isLoading: boolean = true;
  listData: any;
  timeIn: string | undefined;
  timeOut: string | undefined;
  timeOut1: string | undefined;
  totalTime!: string;
  carInput: CarInput = new CarInput();
  typeCard!: string;
  carOutDto: CarOutDto = new CarOutDto();

  constructor(
    private _dataformat: DataFormatService,
    private router: Router,
    private _service: CarService,
    private _toastr: ToastrService,
  ) {
  }

  ngOnInit() {
    this.carOutDto.imgCarIn = 'assets/error.png';
    WebcamUtil.getAvailableVideoInputs().then(
      (mediaDevices: MediaDeviceInfo[]) => {
        this.isCameraExist = mediaDevices && mediaDevices.length > 0;
      }
    );
  }

  handleImageIn(webcamImage: WebcamImage) {
    this.webcamImageIn = webcamImage;
  }

  handleImageOut(webcamImage: WebcamImage) {
    this.webcamImageOut = webcamImage;
  }

  triggerObservableIn(): Observable<void> {
    return this.triggerIn.asObservable();
  }

  triggerObservableOut(): Observable<void> {
    return this.triggerOut.asObservable();
  }

  nextWebcamObservableIn(): Observable<boolean | string> {
    return this.nextWebcamIn.asObservable();
  }

  nextWebcamObservableOut(): Observable<boolean | string> {
    return this.nextWebcamOut.asObservable();
  }

  capPictureIn() {
    this.triggerIn.next();
    this.imageIn = this.webcamImageIn!.imageAsDataUrl;
    this.convertImageIn();
  }

  capPictureOut() {
    this.triggerOut.next();
    this.imageOut = this.webcamImageOut!.imageAsDataUrl;
    this.convertImageOut();
  }

  convertImageIn() {
    this.loading.loading(true);
    this.resultImageIn = ''
    this.isScan = true;
    Tesseract.recognize('/assets/30g.jpg', 'eng').then(({ data: { text } }) => {
      const noSpecialCharacters = text.replace(/[^a-zA-Z0-9]/g, '');
      const checkPlate = /^\d{2}[A-Za-z]\d*$/.test(noSpecialCharacters.toString());
      if (!(noSpecialCharacters.length == 8)) {
        this.resultImageIn = "The number plate is not recognized!"
        this.isScan = false;
        this.loading.loading(false);
        return
      }
      else {
        if (checkPlate) {
          this.resultImageIn = noSpecialCharacters;
          this.loading.loading(false);
          this._service.checkTypeCustomer(this.resultImageIn).subscribe((res) => {
            this.typeCard = res.message;
          });
        }
        else {
          this.resultImageIn = "Invalid license plate!"
          this.isScan = false;
          this.loading.loading(false);
          return
        }
      }
    });
  }

  convertImageOut() {
    this.loading.loading(true);
    this.resultImageOut = ''
    this.isScan = true;
    Tesseract.recognize(this.imageOut, 'eng').then(({ data: { text } }) => {
      const noSpecialCharacters = text.replace(/[^a-zA-Z0-9]/g, '');
      const checkPlate = /^\d{2}[A-Za-z]\d*$/.test(noSpecialCharacters.toString());
      if (!(noSpecialCharacters.length == 8)) {
        this.resultImageOut = "The number plate is not recognized!"
        this.isScan = false;
        this.loading.loading(false);
        return
      } else {
        if (checkPlate) {
          this.resultImageOut = noSpecialCharacters;
          this._service.checkLicensePlate(this.resultImageOut).pipe(finalize(() => {
            this.loading.loading(false);
          })).subscribe((res) => {
            this.carOutDto = res.data;
            this.timeOut = this._dataformat.dateTimeFormat(this.carOutDto.carTimeIn);
            this.timeOut1 = this._dataformat.dateTimeFormat(Date.now());
            const timeIn = new Date(this.carOutDto.carTimeIn);
            const timeOut = new Date();
            const totalTimeInMilliseconds = timeOut.getTime() - timeIn.getTime();
            const totalTimeInHours = totalTimeInMilliseconds / (1000 * 60 * 60);
            this.totalTime = totalTimeInHours.toFixed(2) + ' Hours';
          })
        }
        else {
          this.resultImageIn = "Invalid license plate!"
          this.isScan = false;
          this.loading.loading(false);
          return
        }
      }
    });
  }

  makeid() {
    this.maGuixe = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < 6) {
      this.maGuixe += characters.charAt(Math.floor(Math.random() * charactersLength));
      counter += 1;
    }
    return this.maGuixe;
  }

  takeCar() {
    this.loading.loading(true);
    if (this.isScan) {
      let data = Object.assign(new CarInput, {
        licensePlateIn: this.resultImageIn,
        imgCarIn: this.imageIn,
        typeCard: this.typeCard.toString() == 'Current customers' ? 0 : 1
      });
      this._service.takeCar(data).pipe(finalize(() => {
        this.loading.loading(false);
      })).subscribe((res) => {
        if (res.statusCode == 200) {
          this._toastr.success('Take car succcess');
          this.timeIn = this._dataformat.dateTimeFormat(Date.now());
        }
      });
    }
    else {
      this.loading.loading(false);
      this._toastr.warning('Take car fail, No license plate!')
    }
  }

  carOut() {
    this.loading.loading(true);
    if (this.isScan) {
      const data = Object.assign(new CarOutDto, {
        id: this.carOutDto.id,
        licensePlateOut: this.resultImageOut,
        imgCarOut: this.imageOut
      })
      this._service.carOut(data).pipe((finalize(() => {
        this.loading.loading(false);
      }))).subscribe((res) => {
        if (res.statusCode == 200)
          this._toastr.success("Car export success")
      })
    }
    else {
      this.loading.loading(false);
      this._toastr.warning('Car export fail, No license plate!')
    }
  }
}
