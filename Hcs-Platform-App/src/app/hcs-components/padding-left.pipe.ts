import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'paddingLeft'
})
export class PaddingLeftPipe implements PipeTransform {

  transform(value: any, length: number, c?: string): any {
    if (value !== null && value !== undefined) {
      if (arguments.length === 1) {
        switch (typeof value) {
          case 'string':
            c = ' ';
            break;
          case 'number':
            c = '0';
            break;
          default:
            console.warn('no char for padding');
            return value;
        }
      }
      value = value.toString();
      if (value.length >= length) {
        return value;
      } else {
        const p = length - value.length;
        for (let i = 0; i < p; i++) {
          value = c + value;
        }
        return value;
      }
    }
    return null;
  }

}
