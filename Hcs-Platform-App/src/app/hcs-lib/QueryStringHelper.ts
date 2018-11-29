import { FormGroup } from '@angular/forms';

export class QueryStringHelper {
    constructor() {

    }
    public fillForm(form: FormGroup, queryParams: { [key: string]: any }) {
        Object.keys(queryParams).forEach(k => {
            if (k.startsWith('$')) {
                const name = k.replace(/^\$+/, '');
                const control = form.get(name);
                if (control) {
                    control.setValue(queryParams[k]);
                    if (k.startsWith('$$')) {
                        control.disable();
                    }
                }
            }
        });
    }
}
