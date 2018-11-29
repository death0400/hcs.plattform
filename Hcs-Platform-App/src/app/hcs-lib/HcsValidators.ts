import { AbstractControl } from '@angular/forms';

export module HcsValidators {
    export function maxFileCount(max: number) {
        return function (control: AbstractControl) {
            if (control && control.value && typeof control.value === 'string') {
                if (control.value.split(',').length > max) {
                    return {
                        maxFileCount: { max: max }
                    };
                }
            }
        };
    }
    export function sameAs(compareToControl: AbstractControl, fieldName: string);
    export function sameAs(compareToControl: string, fieldName?: string);
    export function sameAs(compareToControl: string | AbstractControl, fieldName?: string) {
        return function (control: AbstractControl) {
            let compareTo: AbstractControl;
            if (typeof compareToControl === 'string') {
                compareTo = control.parent ? control.parent.get(compareToControl) : undefined;
                if (fieldName === undefined) {
                    fieldName = compareToControl;
                }
            } else {
                compareTo = compareToControl;
            }
            if (compareTo && control.value !== undefined && control.value !== compareTo.value) {
                return { sameAs: { control: fieldName } };
            }
            return null;
        };
    }
}
