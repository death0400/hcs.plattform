import { FormGroup, FormArray } from '@angular/forms';
import { HttpErrorResponse } from '../../../node_modules/@angular/common/http';

export class ErrorHelper {
    public errors: any[] = [];
    public fieldI18nPrefix: string;
    public get hasError() {
        return this.errors.length > 0;
    }
    public clear() {
        this.errors = [];
    }
    public addError(group: FormGroup | FormArray, prefix = '') {
        Object.keys(group.controls).forEach(key => {
            const ctrl = group.controls[key];
            if (ctrl.invalid) {
                if (ctrl instanceof FormGroup || ctrl instanceof FormArray) {
                    this.addError(ctrl, (prefix) + key + '.');

                }
                if (prefix) {
                    console.error(prefix + key, ctrl.errors);
                } else {
                    this.errors.push({
                        field: prefix + (this.fieldI18nPrefix ? (`${this.fieldI18nPrefix}.${key}`) : key),
                        message: ctrl.errors
                    });
                }
            }
        });
    }
    public addHttpError(response: HttpErrorResponse) {
        let error: { [key: string]: { Title: string, Message: string, Data: string }[] };
        if (response.status === 400) {
            error = response.error['ValidationErrors'] ? response.error.ValidationErrors : response.error;
        }
        const buildError = (e: {
            Title: string;
            Message: string;
            Data: string;
        }[]) => {
            const errObj = {};
            e.forEach(x => errObj[x.Message] = { data: (errObj[x.Message] ? errObj[x.Message].data : undefined) || x.Data || true });
            return errObj;
        };
        Object.keys(error).forEach(x => {
            if (error[x] && error[x].length) {
                this.errors.push({
                    field: this.fieldI18nPrefix ? (`${this.fieldI18nPrefix}.${x}`) : x,
                    message: buildError(error[x])
                });
            }
        });
    }
}
