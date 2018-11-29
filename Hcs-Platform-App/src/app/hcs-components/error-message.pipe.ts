import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Pipe({
  name: 'errorMessage', pure: false
})
export class ErrorMessagePipe implements PipeTransform {
  private errorMessage = '';
  constructor(protected translate: TranslateService) {

  }
  transform(errors: ValidationErrors, useTranslate): any {
    if (useTranslate === undefined) {
      useTranslate = true;
    }
    if (errors) {
      const errorKeys = Object.keys(errors);
      if (errorKeys.length) {
        const key = errorKeys[0];
        if (useTranslate) {
          const errorKey = `errors.${key}`;
          const errorInfo = errors[key];
          this.translate.get(errorKey, errorInfo).subscribe(e => {
            this.errorMessage = e === errorKey ? key : e;
          }).unsubscribe();
        } else {
          this.errorMessage = key;
        }
      } else {
        this.errorMessage = '';
      }
      return this.errorMessage;
    } else {
      return '';
    }
  }

}
