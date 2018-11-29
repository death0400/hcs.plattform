import { Pipe, PipeTransform } from '@angular/core';
import * as currency from 'currency.js';
@Pipe({
  name: 'twTax'
})
export class TwTaxPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value !== undefined && value !== null) {
      return parseInt(currency(value).multiply(1.05).value.toFixed(), 10);
    }
    return null;
  }

}
