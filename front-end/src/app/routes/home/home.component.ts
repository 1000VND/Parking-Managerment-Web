import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { WebcamImage, WebcamInitError, WebcamMirrorProperties, WebcamUtil } from 'ngx-webcam';
import { Observable, Subject } from 'rxjs';
import { DataFormatService } from 'src/app/services/data-format.service';
import * as Tesseract from 'tesseract.js';
import { LoadingComponent } from '../common/loading/loading.component';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @ViewChild(LoadingComponent, {static: false}) loading! : LoadingComponent;
  isCollapsed = false;
  isCameraExist = true;
  showWebcam = true;
  webcamImage!: WebcamImage | undefined;
  trigger: Subject<void> = new Subject<void>();
  nextWebcam: Subject<boolean | string> = new Subject<boolean | string>();
  flippedImage: any;
  imga: string | undefined = '/assets/error.png';
  resultImage: string | undefined;
  maGuixe: string | undefined;
  isScan: boolean = true;
  isLoading: boolean = true;

  constructor(
    private _dataformat: DataFormatService,
    private router: Router,
  ) {
  }

  ngOnInit() {
    WebcamUtil.getAvailableVideoInputs().then(
      (mediaDevices: MediaDeviceInfo[]) => {
        this.isCameraExist = mediaDevices && mediaDevices.length > 0;
      }
    );
  }

  submitForm() {
    this.router.navigateByUrl("login");
  }

  handleImage(webcamImage: WebcamImage) {
    this.webcamImage = webcamImage;
  }

  get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  get nextWebcamObservable(): Observable<boolean | string> {
    return this.nextWebcam.asObservable();
  }

  cappicture() {
    this.trigger.next();
    this.imga = this.webcamImage!.imageAsDataUrl;
    this.convertImage();
  }


  changeWWebcam(directionOrDeviceId: boolean | string) {
    this.nextWebcam.next(directionOrDeviceId);
  }

  convertImage() {
    this.loading.loading(true);
    this.resultImage = ''
    this.isScan = true;
    Tesseract.recognize(this.imga!, 'eng').then(({ data: { text } }) => {
      const noSpecialCharacters = text.replace(/[^a-zA-Z0-9]/g, '');
      if (!(noSpecialCharacters.length == 8)) {
        this.resultImage = "Chưa nhận diện được biển số!"
        this.isScan = false;
        this.loading.loading(false);
        return
      }
      this.resultImage = noSpecialCharacters;
      this.loading.loading(false);
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

  inputCar() {
    this.makeid();
  }
}
