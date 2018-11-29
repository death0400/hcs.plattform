import { PipeTransform, Pipe, Inject, LOCALE_ID } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({
  name: 'localDate',
})
export class LocalDatePipe implements PipeTransform {

  static zone: string;
  pipe: DatePipe;
  constructor(@Inject(LOCALE_ID) public local: string) {
    this.pipe = new DatePipe(local);
    if (!LocalDatePipe.zone) {
      const offset = new Date().getTimezoneOffset() * -1;

      let hr = Math.ceil(offset / 60).toFixed(0);
      if (hr.length === 1) {
        hr = '0' + hr;
      }
      let m = (offset % 60).toFixed(0);
      if (m.length === 1) {
        m = '0' + m;
      }
      LocalDatePipe.zone = `${(offset >= 0) ? '+' : '-'}${hr}${m}`;
    }
  }
  transform(value: any, format?: string): string | null {
    return this.pipe.transform(value, format || 'yyyy/MM/dd HH:mm:ss', LocalDatePipe.zone);
  }

}
