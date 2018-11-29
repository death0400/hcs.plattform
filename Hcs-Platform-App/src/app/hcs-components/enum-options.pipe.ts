import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumOptions'
})
export class EnumOptionsPipe implements PipeTransform {

  transform(value: any, args?: any): { value: number, text: string }[] {
    return Object.keys(value).filter((x) => {
      return  !isNaN(parseInt(x, 10));
    }).map(x => {
      return { value: parseInt(x, 10) || 0, text: value[x] };
    });
  }

}
