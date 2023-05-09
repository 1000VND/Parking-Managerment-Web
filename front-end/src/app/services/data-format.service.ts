import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class DataFormatService {

  dateFormat(val: string | moment.Moment | Date) {
    return val ? moment(val).format('DD-MM-YYYY') : '';
  }

  dateTimeFormat(val: any) {
    return val ? moment(val).format('DD-MM-YYYY HH:mm') : '';
  }

  phoneNumberValidate(phoneNumber: string) {
    const PHONE_NUMBER_REGEX = /(0|[+]([0-9]{2})){1}[ ]?[0-9]{2}([-. ]?[0-9]){7}|((([0-9]{3}[- ]){2}[0-9]{4})|((0|[+][0-9]{2}[- ]?)(3|7|8|9|1)([0-9]{8}))|(^[\+]?[(][\+]??[0-9]{2}[)]?([- ]?[0-9]{2}){2}([- ]?[0-9]{3}){2}))$/gm;
    return !phoneNumber || PHONE_NUMBER_REGEX.test(phoneNumber);
  }

  // Tiền
  moneyFormat(value: Number | number | string) {
    return value ? Math.round(Number(value)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,') : '0';
  }

}
